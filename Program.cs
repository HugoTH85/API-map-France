var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/city", (double latitude, double longitude, int distance, int max_pop) =>
{
    List<object>? cities = GetCities(latitude,longitude, distance, max_pop);

    if (cities != null){
        String response = WriteCsv(cities);
        
        return Results.Text(response, "text/csv");
    }
    else{
        return Results.NotFound("Villes non trouvées pour les coordonnées fournies, ou problème survenu lors de la recherche.");
    }
});

app.Run();

//1° longitude ~ 74km
//1° latitude ~ 100km

List<object>? GetCities(double lat, double lon, int distance, int max_pop){
    DBcontext db = new DBcontext();

    string Request="select name,region,latitude,longitude,population,pow(pow(100*(latitude-"+lat.ToString()+"),2)+pow(74*(longitude-"+lon.ToString()+"),2)"+",0.5) from projet_archi.villes where pow(pow(100*(latitude-"+lat.ToString()+"),2)+pow(74*(longitude-"+lon.ToString()+"),2)"+",0.5)<"+distance.ToString()+" and population<="+max_pop.ToString()+";";

    List<object>? Response=db.ExecuteQuery(Request);

    db.CloseConnection();

    return Response;
}

static String WriteCsv(List<object> data){
    String content="\"Ville\",\"Région\",\"Latitude\",\"Longitude\",\"Population\",\"Distance\"";
    foreach (List<object> item in data){
        List<String> temp=[];
        foreach (object thing in item){
            if (thing.GetType()=="test".GetType()){
                temp.Add("\""+thing.ToString()+"\"");
            }
            else{
                temp.Add(thing.ToString());
            }
        }
        content+=Environment.NewLine+string.Join(',',temp);
    }
    return content;
}