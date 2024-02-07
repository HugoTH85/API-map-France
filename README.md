# Carte France Interactive

## Endpoint /city

### Méthode HTTP GET

## Description :
Récupère les coordonnées en latitude et longitude d'un clic de souris avec des filtres établis (distance maximale définissant un rayon, population maximale, Régions prises en compte) et renvoie la liste des villes correspondantes sous format csv.

## Paramètres de requête :
- latitude (obligatoire) : la latitude de la position géographique.
- longitude (obligatoire) : la longitude de la position géographique.
- distance (obligatoire) : la distance maximale (en kilomètres) à partir des coordonnées spécifiées.
- max_pop (obligatoire) : la population maximale que doivent avoir les villes renvoyées.
- regions (optionnel) : une liste de régions à prendre en compte pour filtrer les villes.

## Réponses :
- Code 200 OK : renvoie une liste de villes au format CSV.
- Code 400 Bad Request : indique que des paramètres obligatoires sont manquants ou invalides.
- Code 404 Not Found : indique qu'aucune ville n'a été trouvée pour les paramètres spécifiés.

## Exemple de requête :
http://localhost:5000/city?latitude=48.8566&longitude=2.3522&distance=1000&max_pop=2000000&regions=Ile%20de%20France,Pays%20de%20la%20Loire

## Exemple de réponse :
Ville,Région,Latitude,Longitude,Population,Distance
Nantes,Pays de la Loire,47.2181,-1.5528,314138,332.1903621930187
Angers,Pays de la Loire,47.4736,-0.5542,154508,255.70214166209706
Le Mans,Pays de la Loire,48.0077,0.1984,143252,180.57864509968667
Saint-Nazaire,Pays de la Loire,47.2806,-2.2086,70619,372.48278468245655
La Roche-sur-Yon,Pays de la Loire,46.6705,-1.426,54766,354.907080902301
Cholet,Pays de la Loire,47.06,-0.8783,54186,299.0416624798301
Laval,Pays de la Loire,48.0733,-0.7689,49573,243.88268836833717
Saint-Herblain,Pays de la Loire,47.2122,-1.6497,46352,338.73258648460273
Reze,Pays de la Loire,47.1833,-1.55,42368,333.7413383343534
Saint-Sebastien-sur-Loire,Pays de la Loire,47.2081,-1.5014,27383,329.38642786185056
Orvault,Pays de la Loire,47.2717,-1.6225,26924,334.1110337622064
Saumur,Pays de la Loire,47.26,-0.0769,26599,240.42182503312054
