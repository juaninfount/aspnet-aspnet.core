using SocialMedia.Infraestructure.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;

namespace SocialMedia.Infraestructure.Services
{
    public class UriService: IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            this._baseUri = baseUri;
        }
        public Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl)
        {
            string baseUrl = $"{_baseUri}{actionUrl}";
            return new Uri(baseUrl);
        }
    }

}