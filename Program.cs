var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:3000","http://localhost:3030");
                      });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/city", (double latitude, double longitude, int distance, int max_pop, int number, string regions) =>
{
    List<object>? cities = GetCities(latitude,longitude, distance, max_pop, regions);

    if (cities != null){
        String response = WriteCsv(cities,number);
        
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
    
    string Request="select name,region,latitude,longitude,population,pow(pow(100*(latitude-"+lat.ToString()+"),2)+pow(74*(longitude-"+lon.ToString()+"),2)"+",0.5) as distance from projet_archi2.villes where pow(pow(100*(latitude-"+lat.ToString()+"),2)+pow(74*(longitude-"+lon.ToString()+"),2)"+",0.5)<"+distance.ToString()+" and population<="+max_pop.ToString()+setRequestRegions(RegionFilter)+" order by distance;";

    Console.WriteLine(Request);

    List<object>? Response=db.ExecuteQuery(Request);

    db.CloseConnection();

    return Response;
}

static String WriteCsv(List<object> data,int number){
    String content="Ville,Région,Latitude,Longitude,Population,Distance";
    int i=0;
    foreach (List<object> item in data){
        if (i<number){
            List<String> temp=[];
            foreach (object thing in item){
                temp.Add(thing.ToString());
            }
            content+=Environment.NewLine+string.Join(',',temp);
        }
        else{
            break;
        }
        i++;
    }
    return content;
}

List<string> GetRegions(string str)
{
    if (str!=""){
        List<string> regions = new List<string>();
        string[] parts = str.Split(',');
        foreach (string part in parts){
            regions.Add(part.Trim());
        }
        return regions;
    }
    else {
        return [];
    }
}

String setRequestRegions(List<string> regions){
    String request = "";
    if (regions.Count>0){
        foreach (string region in regions){
            if (region==regions[0]){
                request += " and (region=\""+region+"\"";
            }
            else {
                request += " or region=\""+region+"\"";
            }
        }
        request += ")";
    }
    return request;
}