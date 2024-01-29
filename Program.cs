var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/city", (int latitude, int longitude, int distance, int max_pop) =>
{
    // Utiliser les paramètres de latitude et longitude pour effectuer une requête dans la base de données
    List<object>? response = GetCities(latitude,longitude, distance, max_pop);

    if (response != null){
        return Results.Ok(new { Response = response });
    }
    else{
        return Results.NotFound("Villes non trouvées pour les coordonnées fournies, ou problème survenu lors de la recherche.");
    }
});

app.Run();

//1° longitude ~ 74km
//1° latitude ~ 100km
using System.Drawing.Printing;

List<object>? GetCities(double lat, double lon, int distance, int max_pop){
    DBcontext db = new DBcontext();

    string Request="select name,region,latitude,longitude,population,pow(pow(100*(latitude-"+lat.ToString()+"),2)+pow(74*(longitude-"+lon.ToString()+"),2)"+",0.5) from projet_archi.villes where pow(pow(100*(latitude-"+lat.ToString()+"),2)+pow(74*(longitude-"+lon.ToString()+"),2)"+",0.5)<"+distance.ToString()+" and population<="+max_pop.ToString()+";";

    List<object>? Response=db.ExecuteQuery(Request);

    db.CloseConnection();

    return Response;
}

/*
PARTIE DE TEST DE LA REQUETE MYSQL (mettre en commentaire la partie requête API pour que ça fonctionne bien)

double lat=48.8566;
double lon=2.3522;
int distance=200;
int max_pop=2200000;

List<object>? L=GetCities(lat,lon,distance,max_pop);

if (L!=null){
    foreach (List<object> item in L){
        foreach (object thing in item){
            Console.WriteLine(thing.GetType());
        }
        Console.WriteLine("\n\n");
    }
}
else{
    Console.WriteLine("Liste nulle.");
}
*/