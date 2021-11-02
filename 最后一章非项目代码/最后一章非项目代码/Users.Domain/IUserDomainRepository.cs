using Users.Domain.Events;

namespace Users.Domain;
public interface IUserDomainRepository
{
    Task<User?> FindOneAsync(PhoneNumber phoneNumber);
    Task<User?> FindOneAsync(Guid userId);
    Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string msg);
    Task PublishEventAsync(UserAccessResultEvent eventData);
    Task SavePhoneCodeAsync(PhoneNumber phoneNumber, string code);
    Task<string?> RetrievePhoneCodeAsync(PhoneNumber phoneNumber);
}