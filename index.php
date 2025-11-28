<?php

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
		<script src="functions.js?7"></script>
		<script>
			window.onload = function() {
				start();
			};
		</script>
 	</head>
	<body>
		<div class="container">
			<div class="row border mb-2">
				<h1>The Morrowind Merchant Economy</h1>
				<p>How much money is there in Morrowind and who has it?</p>	
			</div>

			<div class="row mb-2">
			
				<div class="col-sm-2 border">
					<div class="row px-2 pt-2">
						<button class="btn border" onclick="fetchData(this)">Get Data</button>
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
				<div class="col-lg-8">
					<div id="ResultsDiv" class="row p-1"></div>
				</div>
			</div>
			<div class="row border p-1">
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
			</div>
			<div class="row border p-1 text-center">
				<p>This project is open source <a href="https://github.com/anthonylosapio/MorrowindMerchantEconomy">https://github.com/anthonylosapio/MorrowindMerchantEconomy</a></p>
			</div>
		</div>
	</body>
</html>