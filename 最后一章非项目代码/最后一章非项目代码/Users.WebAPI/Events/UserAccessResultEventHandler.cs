using MediatR;
using Users.Domain.Events;

namespace Users.WebAPI.Events
{
    public class UserAccessResultEventHandler
        : INotificationHandler<UserAccessResultEvent>
    {
        private readonly UserApplicationService appService;

        public UserAccessResultEventHandler(UserApplicationService appService)
        {
            this.appService = appService;
        }

        public Task Handle(UserAccessResultEvent notification, CancellationToken cancellationToken)
        {
            var result = notification.Result;
            var phoneNum = notification.PhoneNumber;
            string msg;
            switch(result)
            {
                case Domain.UserAccessResult.OK:
                    msg = $"{phoneNum}登陆成功";
                    break;
                case Domain.UserAccessResult.PhoneNumberNotFound:
                    msg = $"{phoneNum}登陆失败，因为用户不存在";
                    break;
                case Domain.UserAccessResult.PasswordError:
                    msg = $"{phoneNum}登陆失败，密码错误";
                    break;
                case Domain.UserAccessResult.NoPassword:
                    msg = $"{phoneNum}登陆失败，没有设置密码";
                    break;
                case Domain.UserAccessResult.Lockout:
                    msg = $"{phoneNum}登陆失败，被锁定";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return appService.AddNewLoginHistoryAsync(phoneNum,msg);
        }
    }
}
