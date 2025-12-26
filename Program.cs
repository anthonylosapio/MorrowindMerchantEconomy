using System.Text.Json;
using System.Text;

namespace MorrowindJsonParser {
    
    class Npc{
        public string? NpcName {get; set;}
        public string? NpcId {get; set;}
        public string? NpcRace {get; set;}
        public string? NpcClass {get; set;}
        public string? NpcFaction {get; set;}
        public string? NpcLocation {get; set;}
        public string? NpcCellname {get; set;}
        public string? NpcSublocation {get; set;}
        public string? NpcRegion {get; set;}
        public int? NpcGold {get; set;}
    }
    class Cell{
        public string? CellName {get; set;}
        public string? CellFlags {get; set;}
        public string? CellRegion {get; set;}
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
            byte[] s_RegionUtf8 = Encoding.UTF8.GetBytes("region");

            string? NpcId = "";;
            string? NpcName = "";
            string? NpcRace = "";
            string? NpcClass = "";
            string? NpcFaction = "";
            int? NpcGold = 0;

            string? CellName = "";
            string? CellFlags = "";
            string? CellRef = "";
            string? CellRegion = "";

            List<Npc> NpcList = new List<Npc>();
            List<Cell> CellList = new List<Cell>();
            List<string> newCellRefs = new List<string>();

            int state = 0; /*0: waiting to find type
                             1: found/reading npc type object
                             2: adding npc object to list
                             3: found/reading cell type object
                             4: adding cell object to list
            */
            int i = 0;
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
                            CellRegion = "";
                            CellName = "";
                            CellFlags = "";
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
                        if(NpcGold > 0) NpcList.Add(newNpc);
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
                        if(reader.ValueTextEquals(s_RegionUtf8)){
                            reader.Read();
                            CellRegion = reader.GetString();    
                        }
                        if(reader.ValueTextEquals(s_ReferencesUtf8)){//iterate through the refernces objects
                            reader.Read(); // read one item to pass the initial array start token.
                            List<string> NewCellRefs = new List<string>();
                            int closeOut = 1;
                            while(closeOut!=0){
                                reader.Read();
                                if(reader.TokenType==JsonTokenType.EndArray) closeOut--;
                                if(reader.TokenType==JsonTokenType.StartArray) closeOut++;
                                if(reader.TokenType==JsonTokenType.PropertyName){
                                    if(reader.ValueTextEquals(s_IdUtf8)){//get the cell flags
                                        reader.Read();
                                        CellRef = reader.GetString();
                                        if(CellRef.StartsWith("TR_M", StringComparison.OrdinalIgnoreCase)){;
                                            NewCellRefs.Add(CellRef);
                                        }
                                    }
                                }
                            }
                            Cell newCell = new Cell();
                            newCell.CellName = CellName;
                            newCell.CellFlags = CellFlags;
                            newCell.CellRegion = CellRegion;
                            newCell.CellRefs = NewCellRefs;
                            if(NewCellRefs.Count > 0) CellList.Add(newCell);
                            state = 0;
                            
                        }
                    }
                }
            }
            //Add Cell Name & Region to Npc model
            //Do some data cleanup
            foreach (var npc in NpcList){
                npc.NpcClass = npc.NpcClass.Replace("T_Glb_","");
                npc.NpcFaction = npc.NpcFaction.Replace("T_Mw_","");
                npc.NpcFaction = npc.NpcFaction.Replace("TR_Fact_","");
                if(npc.NpcRace=="T_Cnq_Keptu")npc.NpcRace = "Keptu";
                if(npc.NpcRace=="T_Els_Cathay")npc.NpcRace = "Khajiit";
                if(npc.NpcRace=="T_Els_Dagi-raht")npc.NpcRace = "Khajiit";
                if(npc.NpcRace=="T_Els_Ohmes-raht")npc.NpcRace = "Khajiit";
                if(npc.NpcRace=="T_Els_Suthay")npc.NpcRace = "Khajiit";
                if(npc.NpcRace=="T_Hr_Riverfolk")npc.NpcRace = "Riverfolk";

                foreach(var cell in CellList){
                    foreach(var reference in cell.CellRefs){
                        if(npc.NpcId == reference){
                            //Check if location contains comma, and if so split in NpcLocation & NpcSublocation
                            if(cell.CellName.Contains(",")){
                                int delimiterIndex = cell.CellName.IndexOf(",");
                                npc.NpcCellname = cell.CellName.Substring(0, delimiterIndex);
                                npc.NpcSublocation = cell.CellName.Substring(delimiterIndex+1);
                            }else{
                                npc.NpcCellname = cell.CellName;
                            }
                            npc.NpcRegion = cell.CellRegion;
                        }
                    }
                }
            }
            //set the location for each npc, and do a little more cleanup
            foreach(var npc in NpcList){
                if(String.IsNullOrEmpty(npc.NpcCellname)){
                    npc.NpcLocation = npc.NpcRegion;
                }else{
                    npc.NpcLocation = npc.NpcCellname;                    
                }
                if(npc.NpcLocation=="TR_HOLD_Silver Serpent" || npc.NpcLocation=="TR_HOLD_Firewatch Ext Merchants") npc.NpcLocation = "Firewatch";
                if(npc.NpcLocation=="TR_HOLD_Necrom Lighthouse") npc.NpcLocation = "Necrom";
            }

            WriteNpcCsv("TR_Npc.csv", NpcList);
        }

        static void WriteNpcCsv(string filePath, List<Npc> data) {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty.");

            if (data == null || data.Count == 0)
                throw new ArgumentException("No data to write.");

            // Use UTF-8 encoding for compatibility
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8)){
                // Write header
                //writer.WriteLine("Name,Id,Race,Class,Faction,Location,Sublocation,Region, Gold");
                writer.WriteLine("Name,Race,Class,Faction,Gold,Location,Expansion");
                // Write each record
                foreach (var item in data){
                    // Escape commas and quotes if needed
                    string name = EscapeCsvField(item.NpcName);
                    //string id = EscapeCsvField(item.NpcId);
                    string race = EscapeCsvField(item.NpcRace);
                    string npcclass = EscapeCsvField(item.NpcClass);
                    string faction = EscapeCsvField(item.NpcFaction);
                    string location = EscapeCsvField(item.NpcLocation);
                    //string sublocation = EscapeCsvField(item.NpcSublocation);
                    //string region = EscapeCsvField(item.NpcRegion);
                    string gold = EscapeCsvField(item.NpcGold.ToString());
                    string expansion = "Tamriel Rebuilt";

                    //writer.WriteLine($"{name},{id},{race},{npcclass},{faction},{location},{sublocation},{region},{gold}");
                    if(!String.IsNullOrEmpty(location)){
                        writer.WriteLine($"{name},{race},{npcclass},{faction},{gold},{location},{expansion}");
                    }
                }
            }
        }

        static string EscapeCsvField(string field){
            if (field == null) return "";
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n")){
                field = field.Replace("\"", "\"\""); // Escape quotes
                return $"\"{field}\""; //Wrap in quotes 
            }
            return field;
        }
    }
}

