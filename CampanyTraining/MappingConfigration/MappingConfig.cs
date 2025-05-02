namespace CompanyTraining.MappingConfigration
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<CategoryRequest, Category>()
            //    .Map(des => des.Name, src => src.CategoryName)
            //    .Map(des => des.Description, src => src.Note);
            config.NewConfig<RegisterDTO, ApplicationUser>()
                .Map(des => des.UserName, src => src.CompanyName);
            //TypeAdapterConfig<ProfileRequest, ApplicationUser>.NewConfig()
            //    .IgnoreNullValues(true);
            config.NewConfig<ApplicationUser,ProfileResponse>()
             .Map(des => des.CompanyName, src => src.UserName);
            //config.NewConfig<Cart, CartResponse>()
            //    .Map(dest => dest, src => src.Product)
            //    .Map(dest => dest.Count, src => src.Count);
        }
    }
}
