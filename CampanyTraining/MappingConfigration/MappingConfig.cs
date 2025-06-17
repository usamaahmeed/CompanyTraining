namespace CompanyTraining.MappingConfigration
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<CategoryRequest, Category>()
            //    .Map(des => des.Name, src => src.CategoryName)
            //    .Map(des => des.Description, src => src.Note);
            //config.NewConfig<RegisterDTO, ApplicationUser>()
            //    .Map(des => des.UserName, src => src.CompanyName);
            //TypeAdapterConfig<ProfileRequest, ApplicationCompany>.NewConfig()
            //    .IgnoreNullValues(true);
            //config.NewConfig<ApplicationUser, ProfileResponse>()
            // .Map(des => des.CompanyName, src => src.UserName);

            config.NewConfig<Subscribe, CompanyResponse>()
                   .Map(des => des.PackageName, src => src.Package.Name)
                   .Map(des => des.UserName, src => src.ApplicationCompany.UserName)
                   .Map(des => des.Id, src => src.ApplicationUserId);


            config.NewConfig<Course, CourseResponse>()
              .Map(des => des.CategoryName, src => src.Category.Name);

            config.NewConfig<Course,GetCoursesForUserResponse>()
              .Map(des => des.CategoryName, src => src.Category.Name);
            //config.NewConfig<Cart, CartResponse>()
            //    .Map(dest => dest, src => src.Product)
            //    .Map(dest => dest.Count, src => src.Count);

            config.NewConfig<Module, ViewCourseModulesResponse>();

            config.NewConfig<Lesson, GetLessonsResponse>();

            config.NewConfig<ChoiceDto,Choice>();

            config.NewConfig<UpdateChoiceDto, Choice>();

            config.NewConfig<Quiz,GetExamWithQuestionsResponse>();
            config.NewConfig<Question, GetQuestionResponseDto>();
            config.NewConfig<Choice,GetChoiceTextResponse>();
        }
    }
}
