namespace CampanyTraining.MappingConfigration
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<CategoryRequest, Category>()
            //    .Map(des => des.Name, src => src.CategoryName)
            //    .Map(des => des.Description, src => src.Note);

            //config.NewConfig<Cart, CartResponse>()
            //    .Map(dest => dest, src => src.Product)
            //    .Map(dest => dest.Count, src => src.Count);
        }
    }
}
