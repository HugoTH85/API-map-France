var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/city", (int latitude, int longitude) =>
{
    // Utiliser les paramètres de latitude et longitude pour effectuer une requête dans la base de données
    List<object>? response = GetCities(latitude,longitude);

    if (response != null){
        return Results.Ok(new { Response = response });
    }
    else{
        return Results.NotFound("Villes non trouvées pour les coordonnées fournies, ou problème survenu lors de la recherche.");
    }
});

app.MapPost("/city", (int latitude, int longitude) =>
{
    // Utiliser les paramètres de latitude et longitude pour effectuer une requête dans la base de données
    List<object>? response = GetCities(latitude, longitude);

    if (response != null){
        return Results.Ok(new { Response = response });
    }
    else{
        return Results.NotFound("Villes non trouvées pour les coordonnées fournies, ou problème survenu lors de la recherche.");
    }
});

app.Run();

//1° longitude = 74km
//1° latitude = 100km
List<object>? GetCities(int lat, int lon){
    DBcontext db = new DBcontext();
    string Request="select name,region,latitude,longitude,population from projet_archi.villes where pow(pow(100*(latitude-"+lat.ToString()+"),2)+pow(74*(longitude-"+lon.ToString()+"),2)"+",0.5)<200;";

    List<object>? Response=db.ExecuteQuery(Request);
    db.CloseConnection();

    return Response;
}