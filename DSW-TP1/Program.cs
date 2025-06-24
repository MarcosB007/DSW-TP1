
using DSW_TP1.Data;
using DSW_TP1.Persistencia;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace DSW_TP1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // EVITAR ERRORES DE REFERENCIAS CÍCLICAS
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            // HABILITAMOS SWAGGUER
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // LE PASAMOS LA CADENA DE CONEXION A NUESTRO CONTEXTO
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

            var app = builder.Build();

            //CARGAMOS PRODUCTOS EN LA BASE DE DATOS AL INICIAR POR PRIMERA VER EL PROYECTO
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DbInitializer.Seed(context);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {   
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
