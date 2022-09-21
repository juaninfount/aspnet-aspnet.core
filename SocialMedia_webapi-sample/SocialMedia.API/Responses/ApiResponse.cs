using SocialMedia.Core.CustomEntities;

namespace SocialMedia.API.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            this.Data = data;
        }

        public T Data { get; set; }
        public Metadata Metadata{ get; set; }
    }
}
