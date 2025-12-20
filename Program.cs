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

    class Program {
        static void Main() {
            var jsonUtf8Bytes = File.ReadAllBytes("input.json");

            var options = new JsonReaderOptions {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            };

            var reader = new Utf8JsonReader(jsonUtf8Bytes, options);
            
            int i =0;

            byte[] s_typeUtf8 = Encoding.UTF8.GetBytes("type");
            byte[] s_NpcUtf8 = Encoding.UTF8.GetBytes("Npc");
            byte[] s_IdUtf8 = Encoding.UTF8.GetBytes("id");
            byte[] s_NameUtf8 = Encoding.UTF8.GetBytes("name");
            byte[] s_RaceUtf8 = Encoding.UTF8.GetBytes("race");
            byte[] s_ClassUtf8 = Encoding.UTF8.GetBytes("class");
            byte[] s_FactionUtf8 = Encoding.UTF8.GetBytes("faction");
            byte[] s_DataUtf8 = Encoding.UTF8.GetBytes("data");
            byte[] s_GoldUtf8 = Encoding.UTF8.GetBytes("gold");

            string? NpcId = "";;
            string? NpcName = "";
            string? NpcRace = "";
            string? NpcClass = "";
            string? NpcFaction = "";
            int? NpcGold = 0;

            List<Npc> NpcList = new List<Npc>();

            int state = 0; /*0: waiting to find type:npc*/

            while (reader.Read() ) {
                                
                JsonTokenType tokenType = reader.TokenType;

                if(tokenType == JsonTokenType.PropertyName){

                    if(reader.ValueTextEquals(s_typeUtf8) && state==0){ // the property name is "type"
                        reader.Read();
                        if (reader.ValueTextEquals(s_NpcUtf8)){ // the value of type is "npc"
                            state = 1;
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
                        if(reader.ValueTextEquals(s_DataUtf8)){//Npc's data array
                            state = 2;
                        }
                    }
                    if(state==2){
                        if(reader.ValueTextEquals(s_GoldUtf8)){//Npc's gold
                            reader.Read();
                            NpcGold = reader.GetInt16();
                            state = 3;
                        }
                    }
                    if(state==3){
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
                }
            }
            foreach (var item in NpcList){
                Console.WriteLine(item.NpcId + " " + item.NpcName + " " + item.NpcRace + " " + item.NpcClass + " " + item.NpcFaction + " " + item.NpcGold);
            }
        }

    }
}

