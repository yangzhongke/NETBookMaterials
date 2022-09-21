using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zack.EventBus
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, string queueName,
            params Assembly[] assemblies)
        {
            return AddEventBus(services, queueName, assemblies.ToList());
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, string queueName,
            IEnumerable<Assembly> assemblies)
        {
            List<Type> eventHandlers = new List<Type>();
            foreach (var asm in assemblies)
            {
                //用GetTypes()，这样非public类也能注册
                var types = asm.GetTypes().Where(t => t.IsAbstract == false && t.IsAssignableTo(typeof(IIntegrationEventHandler)));
                eventHandlers.AddRange(types);
            }
            return AddEventBus(services, queueName, eventHandlers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="queueName">如果多个消费者订阅同一个Queue，这时Queue中的消息会被平均分摊给多个消费者进行处理，而不是每个消费者都收到所有的消息并处理。为了确保一个应用监听到所有的领域事件，所以不同前端项目的queueName需要不一样。
        /// 因此，对于同一个应用，这个queueName需要保证在多个集群实例和多次运行保持一致，这样可以保证应用重启后仍然能收到没来得及处理的消息。而且这样同一个应用的多个集群实例只有一个能收到一条消息，不会同一条消息被一个应用的多个实例处理。这样消息的处理就被平摊到多个实例中。
        ///</param>
        /// <param name="eventHandlerTypes"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services, string queueName, IEnumerable<Type> eventHandlerTypes)
        {
            foreach (Type type in eventHandlerTypes)
            {
                services.AddScoped(type, type);
            }

            services.AddSingleton<IEventBus>(sp =>
            {
                //如果注册服务的时候就要读取配置，那么可以用AddSingleton的Func<IServiceProvider, TService> 这个重载，
                //因为可以拿到IServiceProvider，省得自己构建IServiceProvider
                var optionMQ = sp.GetRequiredService<IOptions<IntegrationEventRabbitMQOptions>>().Value;
                var factory = new ConnectionFactory()
                {
                    HostName = optionMQ.HostName,

                    DispatchConsumersAsync = true
                };
                if(optionMQ.UserName!=null)
                {
                    factory.UserName = optionMQ.UserName;
                }
                if (optionMQ.Password != null)
                {
                    factory.Password = optionMQ.Password;
                }
                //eventBus归DI管理，释放的时候会调用Dispose
                //eventbus的Dispose中会销毁RabbitMQConnection
                RabbitMQConnection mqConnection = new RabbitMQConnection(factory);
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var eventBus = new RabbitMQEventBus(mqConnection, serviceScopeFactory, optionMQ.ExchangeName, queueName);
                //遍历所有实现了IIntegrationEventHandler接口的类，然后把它们批量注册到eventBus
                foreach (Type type in eventHandlerTypes)
                {
                    //获取类上标注的EventNameAttribute，EventNameAttribute的Name为要监听的事件的名字
                    //允许监听多个事件，但是不能为空
                    var eventNameAttrs = type.GetCustomAttributes<EventNameAttribute>();
                    if (eventNameAttrs.Any() == false)
                    {
                        throw new ApplicationException($"There shoule be at least one EventNameAttribute on {type}");
                    }
                    foreach (var eventNameAttr in eventNameAttrs)
                    {
                        eventBus.Subscribe(eventNameAttr.Name, type);
                    }
                }
                return eventBus;
            });
            return services;
        }
    }
}
