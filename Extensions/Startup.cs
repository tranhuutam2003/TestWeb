namespace TestWeb
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Thêm dịch vụ session
            services.AddControllersWithViews();
            services.AddSession(); // Thêm dòng này để sử dụng session
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Thêm middleware session
            app.UseSession(); // Thêm dòng này để sử dụng session

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
