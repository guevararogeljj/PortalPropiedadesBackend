namespace backend_marketplace.Cors
{
    public static class ConfigureCorse
    {
        public const string CorsPolicyName = "PropiedadesCorse";
        public static void addPropiedadesPublicCors(this IServiceCollection services, string[] origins) => services
            .AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
            {
                builder.WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            }));
    }
}
