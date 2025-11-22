const sections = ['Race','Location','Expansion','Class','Faction'];

function getJSONData(name, value, json, callback){

	var params = "function="+name+"&value="+value+'&json='+json;
	var xhr = new XMLHttpRequest();
	xhr.open("POST", "functions.php", true);
	xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
	xhr.onload = function() {
		if (this.status == 200) {
			callback(this.response);
		};
	};
	xhr.send(params);
}

function start(){
	
	getJSONData("getRaces", "", "", function(data){
		addFilterElements(data);
	});	
	
	getJSONData("getClasses", "", "", function(data){
		addFilterElements(data);
	});
	
	getJSONData("getLocations", "", "", function(data){
		addFilterElements(data);
	});
	
	getJSONData("getExpansions", "", "", function(data){
		addFilterElements(data);
	});
	
	getJSONData("getFactions", "", "", function(data){
		addFilterElements(data);
	});
}

function addFilterElements(data){
	
	container = document.getElementById("filterContainerDiv");
		
	const obj = JSON.parse(data);
		
	var sectionHeaderText = String(Object.keys(obj[0])).replace("[","").replace("'","").replace("]","");
	
	
	const div = document.createElement("div");
	div.id = sectionHeaderText + "DivId";
	div.classList.add("container");
	
	container.appendChild(div);
	
	const header = document.createElement('h1');
	header.id = sectionHeaderText + "HeaderId";
	header.textContent = sectionHeaderText + " +";
	header.classList.add('btn');
	header.setAttribute('section', sectionHeaderText);
	div.appendChild(header);
	
	const collapsingDiv = document.createElement('div');
	collapsingDiv.id = sectionHeaderText + "CollapseId";
	collapsingDiv.classList.add('collapse');
	div.appendChild(collapsingDiv);
	
	header.setAttribute('toggle-target', sectionHeaderText + 'CollapseId');
	header.setAttribute('is-collapsed', '1');
	header.setAttribute('onclick', 'toggleCollapse(this)');
	
	const cardDiv = document.createElement('div');
	collapsingDiv.appendChild(cardDiv);
	
	for(var i=0; i<Object.keys(obj).length; i++){	
		var value;
		for (const key in obj[i]) {
			value = obj[i][key];
		}
		
		const checkbox = document.createElement('input');
		checkbox.type = 'checkbox';
		let id = checkbox.id = sectionHeaderText + i + "CheckBoxId";
		let checkBoxClassName = sectionHeaderText + "CheckBox";
		checkbox.classList.add(checkBoxClassName);
		checkbox.id = id;
		checkbox.checked = true;
		checkbox.value = value;
		
		const label = document.createElement('label');
		label.textContent = value;
		label.htmlFor = id;
			
		cardDiv.appendChild(checkbox);
		cardDiv.appendChild(label);
		cardDiv.appendChild(document.createElement('br'));
	}
	const checkbox = document.createElement('input');
	checkbox.type = 'checkbox';
	checkbox.id = sectionHeaderText + "AllCheckBoxId";
	checkbox.value = 'All';
	checkbox.setAttribute('group', sectionHeaderText);
	checkbox.setAttribute('onchange','selectAllCheckBox(this)');
	checkbox.checked = true;
	
	const label = document.createElement('label');
	label.textContent = 'All';
	label.htmlFor = sectionHeaderText + "AllCheckBoxId";
			
	cardDiv.appendChild(checkbox);
	cardDiv.appendChild(label);
}

function toggleCollapse(h){
	
	var state = h.getAttribute('is-collapsed');
	var targetName = h.getAttribute('toggle-target');
	var target = document.getElementById(targetName);
	var section = h.getAttribute('section');
	
	if(state==1){
		target.classList.remove('collapse');
		h.setAttribute('is-collapsed', '0');
		h.textContent = section + ' -';
	}else{
		target.classList.add('collapse');
		h.setAttribute('is-collapsed', '1');
		h.textContent = section + ' +';
	}
}

function selectAllCheckBox(c){
	var group = c.getAttribute('group');
	var checkBoxClassName = group + 'CheckBox';
	const elements = document.getElementsByClassName(checkBoxClassName);
	if(c.checked){
		for(var i=0; i<elements.length; i++){
			elements[i].checked = true;
		}
	}else{
		for(var i=0; i<elements.length; i++){
			elements[i].checked = false;
		}
	}
}

function fetchData(b){
	b.setAttribute('disabled','true');
	var selectValue = document.getElementById('sortBySelectId').value;
	var excludes = '{';
	var s = 0; //s is iteration index for sections array
	sections.forEach(function(section){
		let x = 0;
		var needsComma = false;
		const checkBoxClassName = section + 'CheckBox'; 
		const elements = document.getElementsByClassName(checkBoxClassName);
		excludes = excludes + '"' + section + '":[{';
		for (let i = 0; i < elements.length; i++) {
			if(elements[i].checked === false){
				if(needsComma){excludes = excludes + ',';}
				excludes = excludes + '"' + x + '":"' + elements[i].value + '"';
				x++;
				needsComma = true;
			}
		}
		excludes = excludes + '}]';
		if(s!= sections.length-1){
			excludes = excludes + ',';
		}
		s++;
	});
	excludes = excludes + '}';

	getJSONData("getData", selectValue, excludes, function(data){
		console.log(data);
		buildTable(data);
		b.disabled = false;
	});
}

function buildTable(json){
	try {
		const obj = JSON.parse(json);
		
		console.log(obj);
		
		
		//get the container
		const container = document.getElementById("ResultsDiv");
		
		//clear out any existing children
		container.innerHTML = '';
		
		//Create columns for each key in first name
		Object.keys(obj[0]).forEach(key => { 
			const colDiv = document.createElement('div');
			colDiv.id = key + '_col';
			colDiv.classList.add('col');
			
			container.appendChild(colDiv);
			
			const rowDiv = document.createElement('div');
			rowDiv.classList.add('row');
			rowDiv.textContent = key;
			
			colDiv.appendChild(rowDiv);
			
			Object.keys(obj).forEach(node => {
				let _value = obj[node][key];
				
				const cellDiv = document.createElement('div');
				cellDiv.textContent = _value;
				cellDiv.classList.add('row');
				colDiv.appendChild(cellDiv);
			});
		});

		//Iterate through all data
		Object.keys(obj).forEach(key => {
			
			Object.keys(obj[key]).forEach(k => {
				
			console.log(`Key2: ${k}, Value: ${obj[key][k]}`);
			
			
			});
		});
	} catch (error) {
		console.error("Error parsing JSON:", error.message);
	}
}
