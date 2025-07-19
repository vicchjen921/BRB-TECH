namespace WebAPI
{
    public static class PolicyRegister
    {
        public static void AddPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanCreateCategory", policy =>
                    policy.RequireClaim("Permission", "CanCreateCategory"));

                options.AddPolicy("CanUpdateCategory", policy =>
                    policy.RequireClaim("Permission", "CanUpdateCategory"));

                options.AddPolicy("CanReadCategories", policy =>
                    policy.RequireClaim("Permission", "CanReadCategories"));

                options.AddPolicy("CanRemoveCategory", policy =>
                    policy.RequireClaim("Permission", "CanRemoveCategory"));


                options.AddPolicy("CanCreateTransaction", policy =>
                    policy.RequireClaim("Permission", "CanCreateTransaction"));

                options.AddPolicy("CanUpdateTransaction", policy =>
                    policy.RequireClaim("Permission", "CanUpdateTransaction"));

                options.AddPolicy("CanReadTransactions", policy =>
                    policy.RequireClaim("Permission", "CanReadTransactions"));

                options.AddPolicy("CanRemoveTransaction", policy =>
                    policy.RequireClaim("Permission", "CanRemoveTransaction"));

                options.AddPolicy("CanGetMonthlyBalance", policy =>
                    policy.RequireClaim("Permission", "CanGetMonthlyBalance"));

                options.AddPolicy("CanCreateDocTransfer", policy =>
                    policy.RequireClaim("Permission", "CanCreateDocTransfer"));
            });
        }
    }
}
