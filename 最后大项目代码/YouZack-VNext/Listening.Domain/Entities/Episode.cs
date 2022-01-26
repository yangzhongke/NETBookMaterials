using Listening.Domain.Events;
using Listening.Domain.Subtitles;
using Listening.Domain.ValueObjects;
using Zack.DomainCommons.Models;

namespace Listening.Domain.Entities;
//EF Core中的实体和DDD的中的实体不一样。DDD中的实体是Data Object，是和数据库表以及字段一一对应的，EF Core中的实体更像领域模型，DO是藏在EF Core框架中的。
//网站上线后，又提出来一个需求“非m4a文件上传后先转码再发布”，如果用面向数据库的开发，就要在Episode表搞一个字段“表示”是否已发布，在发布之前，AudioUrl等属性都是无效的
//按照DDD的思想，就额外拆分出一个“待发布Episode”，转换完成后，再把“待发布Episode”的数据导入Episode，这样就不用对Episode实体做改变。
public record Episode : AggregateRootEntity, IAggregateRoot
{
    private Episode() { }
    public int SequenceNumber { get; private set; }//序号
    public MultilingualString Name { get; private set; }//标题
    public Guid AlbumId { get; private set; }//专辑Id，因为Episode和Album都是聚合根，因此不能直接做对象引用。
    public Uri AudioUrl { get; private set; }//音频路径

    /// <summary>
    ///这是音频的实际长度（秒）
    ///因为IE、旧版Edge、部分手机内置浏览器(小米等)中对于部分音频,
    ///计算的duration以及currentTime和实际的不一致，因此需要根据服务器端
    ///计算出来的实际长度，在客户端做按比例校正
    ///所以服务器端需要储存这个，以便给到浏览器
    /// </summary>
    public double DurationInSecond { get; private set; }//音频时长（秒数）

    //因为启用了<Nullable>enable</Nullable>，所以string是不可空，Migration会默认这个，string?是可空
    public string Subtitle { get; private set; }//原文字幕内容
    public string SubtitleType { get; private set; }//原文字幕格式

    /// <summary>
    /// 用户是否可见（如果发现内部有问题，就先隐藏）
    /// </summary>
    public bool IsVisible { get; private set; }
    /*
    public static Episode Create(Guid id, int sequenceNumber, MultilingualString name, Guid albumId, Uri audioUrl,
        double durationInSecond, string subtitleType, string subtitle)
    {
        var parser = SubtitleParserFactory.GetParser(subtitleType);
        if (parser == null)
        {
            throw new ArgumentOutOfRangeException(nameof(subtitleType), $"subtitleType={subtitleType} is not supported.");
        }

        //新建的时候默认可见
        Episode episode = new Episode()
        {
            Id = id,
            AlbumId = albumId,
            DurationInSecond = durationInSecond,
            AudioUrl = audioUrl,
            Name = name,
            SequenceNumber = sequenceNumber,
            Subtitle = subtitle,
            SubtitleType = subtitleType,
            IsVisible = true
        };
        episode.AddDomainEvent(new EpisodeCreatedEvent(episode));
        return episode;
    }*/

    public Episode ChangeSequenceNumber(int value)
    {
        this.SequenceNumber = value;
        this.AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
        return this;
    }

    public Episode ChangeName(MultilingualString value)
    {
        this.Name = value;
        this.AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
        return this;
    }

    public Episode ChangeSubtitle(string subtitleType, string subtitle)
    {
        var parser = SubtitleParserFactory.GetParser(subtitleType);
        if (parser == null)
        {
            throw new ArgumentOutOfRangeException(nameof(subtitleType), $"subtitleType={subtitleType} is not supported.");
        }
        this.SubtitleType = subtitleType;
        this.Subtitle = subtitle;
        this.AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
        return this;
    }

    public Episode Hide()
    {
        this.IsVisible = false;
        this.AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
        return this;
    }
    public Episode Show()
    {
        this.IsVisible = true;
        this.AddDomainEventIfAbsent(new EpisodeUpdatedEvent(this));
        return this;
    }

    public override void SoftDelete()
    {
        base.SoftDelete();
        this.AddDomainEvent(new EpisodeDeletedEvent(this.Id));
    }

    public IEnumerable<Sentence> ParseSubtitle()
    {
        var parser = SubtitleParserFactory.GetParser(this.SubtitleType);
        return parser.Parse(this.Subtitle);
    }
    public class Builder
    {
        private Guid id;
        private int sequenceNumber;
        private MultilingualString name;
        private Guid albumId;
        private Uri audioUrl;
        private double durationInSecond;
        private string subtitle;
        private string subtitleType;
        public Builder Id(Guid value)
        {
            this.id = value;
            return this;
        }
        public Builder SequenceNumber(int value)
        {
            this.sequenceNumber = value;
            return this;
        }
        public Builder Name(MultilingualString value)
        {
            this.name = value;
            return this;
        }
        public Builder AlbumId(Guid value)
        {
            this.albumId = value;
            return this;
        }
        public Builder AudioUrl(Uri value)
        {
            this.audioUrl = value;
            return this;
        }
        public Builder DurationInSecond(double value)
        {
            this.durationInSecond = value;
            return this;
        }
        public Builder Subtitle(string value)
        {
            this.subtitle = value;
            return this;
        }
        public Builder SubtitleType(string value)
        {
            this.subtitleType = value;
            return this;
        }
        public Episode Build()
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (albumId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }
            if (audioUrl == null)
            {
                throw new ArgumentNullException(nameof(audioUrl));
            }
            if (durationInSecond <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(durationInSecond));
            }
            if (subtitle == null)
            {
                throw new ArgumentNullException(nameof(subtitle));
            }
            if (subtitleType == null)
            {
                throw new ArgumentNullException(nameof(subtitleType));
            }
            Episode e = new Episode();
            e.Id = id;
            e.SequenceNumber = sequenceNumber;
            e.Name = name;
            e.AlbumId = albumId;
            e.AudioUrl = audioUrl;
            e.DurationInSecond = durationInSecond;
            e.Subtitle = subtitle;
            e.SubtitleType = subtitleType;
            e.IsVisible = true;
            e.AddDomainEvent(new EpisodeCreatedEvent(e));
            return e;
        }
    }
}
