<?php
include 'config.php';
?>
<!DOCTYPE html>
<html land="en">
	<head>
		<meta http-equiv=“Pragma” content=”no-cache”>
		<meta http-equiv=“Expires” content=”-1″>
		<meta http-equiv=“CACHE-CONTROL” content=”NO-CACHE”>
		<title>The Morrowind Merchant Economy</title>
		<meta charset="utf-8">
		<meta name="viewport" content="width=device-width, initial-scale=1">
		<link href="bootstrap.css" rel="stylesheet">
		<script src="functions.js?4"></script>
		<script>
			window.onload = function() {
				start();
			};
		</script>
		<style>
			.content{
				background-color: #dcc081;
				//border: solid 2px #6F541D
			}
			body{
				background-image: url("img/bg.avif");
			}
			a:link {
				color: purple;
				text-decoration: none;
			}
			a:visited {
				color: indigo;
			}

			a:hover {
				color: orange;
				text-decoration: underline;
			}
			a:active {
				color: red;
			}
			input[type="checkbox"] {
				accent-color: purple;
			}
			.PlusMinusSpan{
				font-weight: 800;
				font-size: 1.1rem;
			}
			.btn:hover {
				outline: 1px solid orange;
			}
			.filterBtn{
				width: 100%;
				text-align: left;
			}
			.selectedTab{
				background-color: #dbac64;
				border-top: 1px solid black;
				border-right: 1px solid black;
				border-left: 1px solid black;
				border-bottom: none;
				border-top-left-radius: 10px;
				border-top-right-radius: 10px;
				border-bottom-left-radius: 0px;
				border-bottom-right-radius: 0px;
			}
			.unselectedTab{
				background-color: #dbac64;
				border-top-left-radius: 10px;
				border-top-right-radius: 10px;
				border-bottom-left-radius: 0px;
				border-bottom-right-radius: 0px;
			}
			.dataTable{
				margin-left: auto;
				margin-right: auto
				/*padding-left: 5px;
				padding-right: 5px;*/
				border-collapse: collapse;
				width: 98%;
			}
			.dataCell{
				border-bottom: 1px solid #A9A79E;
				border-left: 1px solid #A9A79E;
			}
			thead th {
				position: sticky;
				top: 0;
				background-color: #dcc081;
				z-index: 10;
			}
		</style>
 	</head>
	<body style="background-color: #2B1A08;">
		<div class="container">
			<div class="row border mb-2 content">
				<h1>The Morrowind Merchant Economy</h1>
				<p>How much money is there in Morrowind and who has it?</p>	
			</div>

			<div class="row mb-2">
			
				<div class="col-sm-2 border content">
					<div class="row px-2 pt-2">
						<button class="btn border" style="background-color: #dbac64;" onclick="fetchData(this)">Get Data</button>
					</div>
					<div class="row px-3">
						 <label for="showSelectId">Show:</label><br><select id="showSelectId">
							<option value="Race">Race</option>
							<option value="Location">Location</option>
							<option value="Expansion">Expansion</option>
							<option value="Class">Class</option>
							<option value="Faction">Faction</option>
						 </select>
					</div>
					<div class="row px-3">
						<label for="sortBySelectId">Sort By:</label><br><select id="sortBySelectId">
							<option value="Total Gold">Total Gold</option>
							<option value="Total Merchants">Total Merchants</option>
							<option value="Average Gold">Average Gold</option>
						 </select>					
					</div>
					<div class="row" id="filterContainerDiv">
					<p>Include / Exclude</p>
					</div>				
				</div>
				<div class="col">
					<div id="ResultsDiv" class="row p-1" style="overflow-x: auto; overflow-y: auto; max-height: 700px;">
						<div class="content">
							How to use this site.
							<ul>
								<li>Choose how you want the aggregated data to be grouped by selecting Race, Class, Location, Expansion, or Faction.</li>
								<li>Choose how you want the aggregated data to be sorted (always descending) either by Total Gold, Total Merchants, or Average Gold.</li>
								<li>Expand the Race, Class, Expansion, Faction, & Location section to de-select anything you want to exclude from the final results & aggregation (for example, if you only want to see data for the base game, expand the "Expansion" section and un-select Tribunal & Bloodmoon).</li>
								<li>Click the Get Data button</li>
								<li>The results will have 2 tabs, one with the aggregated data and one containing the list of all merchants
								that were included in the calculation.</li>
							</ul>
							<p>As an example, if you want to see which Faction has the most merchants in Vivec city you would:</p>
							<ul>
								<li>Select <b>Faction</b> from the Show drop down.</li>
								<li>Select <b>Total Merchants</b> from the Sort By Drop Down.</li>
								<li>Expand <b>Location</b> and de-select everything but Vivec.</li>
								<li>Click the Get Data button</li>
							</ul>
						</div>
					</div>
				</div>
			</div>
			<div class="row p-1 content">
				<div class="col">
					<p>Data for this project was obtained from:<br>
					<a href="https://en.uesp.net/wiki/Morrowind:Merchants" target="_blank">https://en.uesp.net/wiki/Morrowind:Merchants</a><br>
					<a href="https://en.uesp.net/wiki/Tribunal:Services#Merchants" target="_blank">https://en.uesp.net/wiki/Tribunal:Services#Merchants</a><br>
					<a href="https://en.uesp.net/wiki/Bloodmoon:Merchants" target="_blank">https://en.uesp.net/wiki/Bloodmoon:Merchants</a></p>					
				</div>
				<div class="col">
					<p>Notes about the data:<br>
					<ul>
						<li>Todd Test and Lord Cluttermonkey merchants are not included in the data because they do not appear in the actual game.</li>
						<li>All Tribunal merchants have their location set to Mournhould</li>
						<li>Both traders in Raven Rock are included even though you can only get one of them in game.</li>
					</ul>
					</p>
				</div>
				<div class="row" style="text-align: center;">
				<p>If anyone knows where I can find merchant data for Tamriel Rebuilt (or other large landmass mods) please let me know.<br>
				See something wrong with the data or the site? Let me know! <a href="mailto: morrowind@losapio.cc">morrowind@losapio.cc</a></p>
				</div>				
			</div>
			<div class="row text-center">
				<div class="col border my-1 text-center content">
					<p>This project is open source <a href="https://github.com/anthonylosapio/MorrowindMerchantEconomy">https://github.com/anthonylosapio/MorrowindMerchantEconomy</a></p>
					<p>This data has been queried <span style="font-weight: bold;">
					<?php 
						try{
							$sql = "SELECT COUNT(1) FROM logs";
							$result = $con->query($sql);
							$row   = mysqli_fetch_row($result);
							echo $row[0];
						}catch(Exception $e){
							echo $e->getMessage();
						}
					?>
					</span> times.
					</p>
				</div>
			</div>
		</div>
	</body>
</html>