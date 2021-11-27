using AutoMapper;

namespace SwaggerDocumentation.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile() 
        {
            CreateMap<Models.BookCreation, Entities.Book>();
            CreateMap<Models.BookCreationWithAmountOfPages, Entities.Book>();
        }
    }
}
