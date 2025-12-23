using System.Text.Json;
using System.Text;

namespace MorrowindJsonParser {
    
    class Npc{
        public string? NpcName {get; set;}
        public string? NpcId {get; set;}
        public string? NpcRace {get; set;}
        public string? NpcClass {get; set;}
        public string? NpcFaction {get; set;}
        public int? NpcGold {get; set;}
    }
    class Cell{
        public string? CellName {get; set;}
        public string? CellFlags {get; set;}
        public List<string>? CellRefs {get; set;}
    }

    class Program {
        static void Main() {
            var jsonUtf8Bytes = File.ReadAllBytes("input.json");

            var options = new JsonReaderOptions {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            };

            var reader = new Utf8JsonReader(jsonUtf8Bytes, options);

            byte[] s_typeUtf8 = Encoding.UTF8.GetBytes("type");
            byte[] s_NpcUtf8 = Encoding.UTF8.GetBytes("Npc");
            byte[] s_IdUtf8 = Encoding.UTF8.GetBytes("id");
            byte[] s_NameUtf8 = Encoding.UTF8.GetBytes("name");
            byte[] s_RaceUtf8 = Encoding.UTF8.GetBytes("race");
            byte[] s_ClassUtf8 = Encoding.UTF8.GetBytes("class");
            byte[] s_FactionUtf8 = Encoding.UTF8.GetBytes("faction");
            byte[] s_DataUtf8 = Encoding.UTF8.GetBytes("data");
            byte[] s_GoldUtf8 = Encoding.UTF8.GetBytes("gold");
            byte[] s_CellUtf8 = Encoding.UTF8.GetBytes("Cell");
            byte[] s_FlagsUtf8 = Encoding.UTF8.GetBytes("flags");
            byte[] s_ReferencesUtf8 = Encoding.UTF8.GetBytes("references");

            string? NpcId = "";;
            string? NpcName = "";
            string? NpcRace = "";
            string? NpcClass = "";
            string? NpcFaction = "";
            int? NpcGold = 0;

            string? CellName = "";
            string? CellFlags = "";
            string? CellRef = "";

            List<Npc> NpcList = new List<Npc>();
            List<Cell> CellList = new List<Cell>();
            List<string> CellRefs = new List<string>();

            int state = 0; /*0: waiting to find type
                             1: found/reading npc type object
                             2: adding npc object to list
                             3: found/reading cell type object
                             4: adding cell object to list
            */

            while (reader.Read() ) {
                                
                JsonTokenType tokenType = reader.TokenType;

                if(tokenType == JsonTokenType.PropertyName){

                    if(reader.ValueTextEquals(s_typeUtf8) && state==0){ // the property name is "type"
                        reader.Read();
                        if (reader.ValueTextEquals(s_NpcUtf8)){ // the value of type is "Npc"
                            state = 1;
                        }
                        if (reader.ValueTextEquals(s_CellUtf8)){ // the value of type is "Cell"
                            state = 3;
                        }
                    }

                    if (state == 1){
                        if(reader.ValueTextEquals(s_IdUtf8)){//Npc's id
                            reader.Read();
                            NpcId = reader.GetString();
                        }
                        if(reader.ValueTextEquals(s_NameUtf8)){//Npc's name
                            reader.Read();
                            NpcName = reader.GetString();
                        }
                        if(reader.ValueTextEquals(s_RaceUtf8)){//Npc's race
                            reader.Read();
                            NpcRace = reader.GetString();
                        }
                        if(reader.ValueTextEquals(s_ClassUtf8)){//Npc's class
                            reader.Read();
                            NpcClass = reader.GetString();
                        }
                        if(reader.ValueTextEquals(s_FactionUtf8)){//Npc's faction
                            reader.Read();
                            NpcFaction = reader.GetString();
                            if(String.IsNullOrEmpty(NpcFaction)){
                                NpcFaction = "None";
                            }
                        }
                        if(reader.ValueTextEquals(s_GoldUtf8)){//Npc's gold
                            reader.Read();
                            NpcGold = reader.GetInt16();
                            state = 2;
                        }

                    }

                    if(state==2){
                        Npc newNpc = new Npc();
                        newNpc.NpcId = NpcId;
                        newNpc.NpcName = NpcName;
                        newNpc.NpcRace = NpcRace;
                        newNpc.NpcClass = NpcClass;
                        newNpc.NpcFaction = NpcFaction;
                        newNpc.NpcGold = NpcGold;
                        NpcList.Add(newNpc);
                        state = 0;
                        NpcGold = 0;
                    }

                    if (state == 3){ //Reading data from Cell object
                        if(reader.ValueTextEquals(s_NameUtf8)){//Cell's Name
                            reader.Read();
                            CellName = reader.GetString();
                        }
                        if(reader.ValueTextEquals(s_DataUtf8)){//iterate through the data array
                            while(reader.TokenType!=JsonTokenType.EndArray){
                                reader.Read();
                                if(reader.TokenType==JsonTokenType.PropertyName){
                                    if(reader.ValueTextEquals(s_FlagsUtf8)){//get the cell flags
                                        reader.Read();
                                        CellFlags = reader.GetString();
                                    }
                                }
                            }
                            while(reader.TokenType!=JsonTokenType.PropertyName){reader.Read();}//continue reading till next property
                        }
                        if(reader.ValueTextEquals(s_ReferencesUtf8)){//iterate through the refernces objects
                            reader.Read(); // read one item to pass the initial array start token.
                            CellRefs.Clear();
                            int closeOut = 1;
                            while(closeOut!=0){
                                reader.Read();
                                if(reader.TokenType==JsonTokenType.EndArray){closeOut--;}
                                if(reader.TokenType==JsonTokenType.StartArray){closeOut++;}
                                if(reader.TokenType==JsonTokenType.PropertyName){
                                    if(reader.ValueTextEquals(s_IdUtf8)){//get the cell flags
                                        reader.Read();
                                        CellRef = reader.GetString();
                                        if(CellRef.StartsWith("TR_M", StringComparison.OrdinalIgnoreCase)){;
                                            CellRefs.Add(CellRef);
                                        }
                                    }
                                }
                            }
                            state = 4;
                        }
                    }
                    if(state==4){
                        Cell newCell = new Cell();
                        newCell.CellName = CellName;
                        newCell.CellFlags = CellFlags;
                        newCell.CellRefs = CellRefs;
                        CellList.Add(newCell);
                        state = 0;
                     }
                }
            }

            // foreach (var item in NpcList){
            //     if(item.NpcGold > 0){
            //         //Console.WriteLine(item.NpcId + " " + item.NpcName + " " + item.NpcRace + " " + item.NpcClass + " " + item.NpcFaction + " " + item.NpcGold);
            //     }
            // }
            
            WriteNpcCsv("TR_Npc.csv", NpcList);

            foreach(var item in CellList){
                foreach(var reference in item.CellRefs){
                    Console.WriteLine(item.CellName + " " + item.CellFlags + " " + reference);
                }
            }
        }

    

        static void WriteNpcCsv(string filePath, List<Npc> data) {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty.");

            if (data == null || data.Count == 0)
                throw new ArgumentException("No data to write.");

            // Use UTF-8 encoding for compatibility
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8)){
                // Write header
                writer.WriteLine("Name,Id,Race,Class,Faction,Gold");

                // Write each record
                foreach (var item in data){
                    // Escape commas and quotes if needed
                    string name = EscapeCsvField(item.NpcName);
                    string id = EscapeCsvField(item.NpcId);
                    string race = EscapeCsvField(item.NpcRace);
                    string npcclass = EscapeCsvField(item.NpcClass);
                    string faction = EscapeCsvField(item.NpcFaction);
                    string gold = EscapeCsvField(item.NpcGold.ToString());

                    writer.WriteLine($"{name},{id},{race},{npcclass},{faction},{gold}");
                }
            }
        }

        static string EscapeCsvField(string field)
        {
            if (field == null) return "";
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n")){
                field = field.Replace("\"", "\"\""); // Escape quotes
                return $"\"{field}\""; //Wrap in quotes 
            }
            return field;
        }
    }
}

