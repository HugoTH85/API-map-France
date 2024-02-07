var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/city", (double latitude, double longitude, int distance, int max_pop, string regions) =>
{
    List<object>? cities = GetCities(latitude,longitude, distance, max_pop, regions);

    if (cities != null){
        String response = WriteCsv(cities);
        
        return Results.Text(response);
    }
    else{
        return Results.NotFound("Villes non trouvées pour les coordonnées fournies, ou problème survenu lors de la recherche.");
    }
});

app.Run();

//1° longitude ~ 74km
//1° latitude ~ 100km

List<object>? GetCities(double lat, double lon, int distance, int max_pop, string regions){
    DBcontext db = new DBcontext();

    List<string> RegionFilter=GetRegions(regions);
    
    string Request="select name,region,latitude,longitude,population,pow(pow(100*(latitude-"+lat.ToString()+"),2)+pow(74*(longitude-"+lon.ToString()+"),2)"+",0.5) from projet_archi2.villes where pow(pow(100*(latitude-"+lat.ToString()+"),2)+pow(74*(longitude-"+lon.ToString()+"),2)"+",0.5)<"+distance.ToString()+" and population<="+max_pop.ToString()+setRequestRegions(RegionFilter)+";";

    Console.WriteLine(Request);

    List<object>? Response=db.ExecuteQuery(Request);

    db.CloseConnection();

    return Response;
}

static String WriteCsv(List<object> data){
    String content="Ville,Région,Latitude,Longitude,Population,Distance";
    foreach (List<object> item in data){
        List<String> temp=[];
        foreach (object thing in item){
            temp.Add(thing.ToString());
        }
        content+=Environment.NewLine+string.Join(',',temp);
    }
    return content;
}

List<string> GetRegions(string str)
{
    List<string> regions = new List<string>();
    string[] parts = str.Split(',');
    foreach (string part in parts){
        regions.Add(part.Trim());
    }
    return regions;
}

String setRequestRegions(List<string> regions){
    String request = "";
    foreach (string region in regions){
        if (region==regions[0]){
            request += " and (region=\""+region+"\"";
        }
        else {
            request += " or region=\""+region+"\"";
        }
    }
    request += ")";
    return request;
}