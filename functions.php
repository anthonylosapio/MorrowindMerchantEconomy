<?php
include 'config.php';

if(isset($_POST["function"])){$function = $_POST["function"];}else{$function = null;}
if(isset($_POST["value"])){$value = $_POST["value"];}else{$value = null;}
if(isset($_POST["json"])){$json = $_POST["json"];}else{$json = null;}

$options = array('Dark Elf',
'Breton',
'Nord',
'Imperial',
'Khajiit',
'Redguard',
'High Elf',
'Wood Elf',
'Orc',
'Scamp',
'Argonian',
'Mudcrab',
'Trader Service',
'Alchemist Service',
'Healer Service',
'Publican',
'Clothier',
'Smith',
'Enchanter Service',
'Pawnbroker',
'Thief Service',
'Apothecary Service',
'Barbarian',
'Priest Service',
'Noble',
'Savant Service',
'Assassin Service',
'Bookseller',
'Creature',
'Trader',
'Thief',
'Agent',
'Commoner',
'Wise Woman Service',
'Alchemist',
'Spellsword',
'Sorcerer',
'Enchanter',
'Monk',
'Guard',
'Ashlanders ',
'None',
'Mages Guild ',
'House Redoran ',
'House Telvanni ',
'Thieves Guild ',
'House Hlaalu ',
'Imperial Legion ',
'Camonna Tong ',
'Quarra Clan ',
'Berne Clan ',
'Imperial Cult ',
'Tribunal Temple ',
'Morag Tong ',
'Fighters Guild ',
'Aundae Clan ',
'Blades ',
'East Empire Company',
'Skaal',
'Zainab Camp',
'Valenvaryon',
'Erabenimsun Camp',
'Ebonheart',
'Vivec',
'Balmora',
'Maar Gan',
'Tel Vos',
'Ald\'ruhn',
'Moonmoth Legion Fort',
'Gnaar Mok',
'Sadrith Mora',
'Druscashti',
'Galom Daeus',
'Buckmoth Legion Fort',
'Seyda Neen',
'Tel Aruhn',
'Gnisis',
'Suran',
'Wolverine Hall',
'Ghostgate',
'Molag Mar',
'Tel Mora',
'Vos',
'Caldera',
'Hla Oad',
'Pelagiad',
'Tel Branora',
'Holamayan Monastery',
'Dagon Fel',
'Indarys Manor',
'Ashmelech',
'Tel Uvirith',
'Dren Plantation',
'Rethan Manor',
'Urshilaku Camp',
'Ahemmusa Camp',
'Azura\'s Coast Region',
'Ald Velothi',
'Bitter Coast Region',
'Khuul',
'Mournhold',
'Raven Rock',
'Thirsk',
'Fort Frostmoth',
'Skaal Village',
'Base Game',
'Tribunal',
'Bloodmoon');

$sections = array('Race','Location','Expansion','Class','Faction');

if($function=="getRaces" || $function=="getClasses" || $function=="getLocations" || $function=="getExpansions" || $function=="getFactions"){
	
	$filter = "";
	if($function=="getRaces"){ $filter="Race"; }
	if($function=="getClasses"){ $filter="Class"; }
	if($function=="getLocations"){ $filter="Location"; }
	if($function=="getExpansions"){ $filter="Expansion"; }
	if($function=="getFactions"){ $filter="Faction"; }
	
	$sql = "SELECT DISTINCT $filter FROM morrowindeconomy";
	try{
		$result = $con->query($sql);
		$rows = array();
		while($row = $result->fetch_assoc()) {
			$rows[] = $row;
		}
		echo json_encode($rows);
	}catch(Exception $e){
		echo $e->getMessage();
	}
}

if($function=="getData"){

	$orderby = "";
	if($value=="Race"){$orderby = $sections[0];}
	if($value=="Location"){$orderby = $sections[1];}
	if($value=="Expansion"){$orderby = $sections[2];}
	if($value=="Class"){$orderby = $sections[3];}
	if($value=="Faction"){$orderby = $sections[4];}

	$j = json_decode($json, true);
	
	$where = "1=1";
	
	foreach($sections as $section){
		
		if( count($j[$section][0]) > 0){
			
			$where = $where . " AND $section NOT IN (";
			for($i=0; $i<count($j[$section][0]); $i++){
				$key = "$i";
				$value = $j[$section][0][$key];
				if( $i>0 ){ $where = $where . ","; }
				$where = $where . "'" . $value . "'";
			}
			$where = $where . ")";	
			
		}
		
	}
	
	
	
	$sql = "SELECT $orderby, COUNT(1) AS `Total Merchants`, SUM(Gold) AS Gold FROM morrowindeconomy WHERE $where GROUP BY $orderby ORDER BY SUM(Gold) DESC";
//	echo $sql;
	
	try{
		$result = $con->query($sql);
		$rows = array();
		while($row = $result->fetch_assoc()) {
			$rows[] = $row;
		}
		echo json_encode($rows);
	}catch(Exception $e){
//		echo $e->getMessage();
	}
}

$con->close();

?>