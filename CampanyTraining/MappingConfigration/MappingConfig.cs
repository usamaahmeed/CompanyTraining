namespace CompanyTraining.MappingConfigration
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<CategoryRequest, Category>()
            //    .Map(des => des.Name, src => src.CategoryName)
            //    .Map(des => des.Description, src => src.Note);
            config.NewConfig<RegisterDTO, ApplicationCompany>()
                .Map(des => des.UserName, src => src.CompanyName);
            //TypeAdapterConfig<ProfileRequest, ApplicationCompany>.NewConfig()
            //    .IgnoreNullValues(true);
            config.NewConfig<ApplicationCompany,ProfileResponse>()
             .Map(des => des.CompanyName, src => src.UserName);
            //config.NewConfig<Cart, CartResponse>()
            //    .Map(dest => dest, src => src.Product)
            //    .Map(dest => dest.Count, src => src.Count);
        }
    }
}
