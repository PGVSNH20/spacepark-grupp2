# Documentation

Use this file to fill in your documentation
1. SpacePark - Vad behöver vi för att få den här grejen att fungera?
- Parkering, ska kunna stängas och öppnas beroende på om det är fullt.
- Klasser för vad entity framework kommer hantera. 
- Klasser som PersonClass, VehicleClass, ParkingClass (kunna koppla user och vehicle).
- Interface (IVehicle) med bool som kollar så att användaren har ett starship. Bool som kollar om det finns lediga parkeringsplatser. 
- Enhetstester
- Kolla vilken typ av person som behöver parkera
- Kolla vilket typ av fordon?
- Färdig lista med Starships så användare kan välja vilket fordon som ska parkeras. Får fortfarande registrera sig, men inte parkera om användaren försöker parkera fel fordon.

UX
Vad ska synas på skärmen?
Språk på applikationen? R2D2.
Interagera med applikationen? Muspekare? Tangentbord?
Ska vi ha en meny?

Användaren ska kunna gå in på applikationen och parkera. 
Kunna skriva in sitt namn. 

TODO:
- Använda EF och code first för att skriva till databasen. (false)
- Testa RestSharp. (false)
- Kunna prata med API / Swapi vilka saker som stämmer. (false)
- Behöver ett betalningssystem. (false)
- Kunna ta emot userinput. (false)
- Skapa lite enhetstester. (false)

Fabian skapade ett grafiskt UI som vi kan använda oss av för projektet. 

Er-diagram? :(
Skapa en databas med hjälp av EF. 
Databasen - vad behöver vi?
- Namn
- Parkeringsavgift
- FordonsID
- AnvändarID

- Tabell med olika fordon.

- Info om själva parkeringen. 
- ParkeringsID

Kicka användaren om hen inte är med i StarWarsfilmerna. Kicka användaren om hen försöker parkera ett fordon som inte är med i StarWarsfilmerna.
