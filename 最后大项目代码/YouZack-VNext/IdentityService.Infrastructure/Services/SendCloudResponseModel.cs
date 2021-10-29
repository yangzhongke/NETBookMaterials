namespace IdentityService.Infrastructure.Services
{
    class SendCloudResponseModel
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
