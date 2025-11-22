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
		<script src="functions.js?3"></script>
		<script>
			window.onload = function() {
				start();
			};
		</script>
 	</head>
	<body>
		<div class="container">
			<div class="row">
				<h1>The Morrowind Merchant Economy</h1>
				<p>How much money is there in Morrowind and who has it?</p>
				<p>Coming soon.</p>			
			</div>

			<div class="row">
			
				<div class="col-sm">
					<button onclick="fetchData(this)">Get Data</button>
					<div class="flex">

						 <label for="sortBySelectId">Show:</label><br><select id="sortBySelectId">
							<option value="Race">Race</option>
							<option value="Location">Location</option>
							<option value="Expansion">Expansion</option>
							<option value="Class">Class</option>
							<option value="Faction">Faction</option>
						 </select>
					</div>
					
					<div class="row" id="filterContainerDiv">
					Include / Exclude
					</div>				
				</div>
				<div class="col-lg">

					<div id="ResultsDiv" class="row"></div>
				</div>
			</div>
		</div>
	</body>
</html>