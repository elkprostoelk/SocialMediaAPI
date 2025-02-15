namespace SocialMediaAPI.Common.DTO
{
    public class ServiceResultDto
    {
        public bool Success { get; set; } = true;

        public List<string> Errors { get; set; } = [];
    }

    public class ServiceResultDto<T> : ServiceResultDto
    {
        public T? Result { get; set; }
    }
}
