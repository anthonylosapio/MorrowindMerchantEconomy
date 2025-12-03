const sections = ['Race','Location','Expansion','Class','Faction'];

function getJSONData(name, value, sort, json, callback){

	var params = "function="+name+"&value="+value+'&sort='+sort+'&json='+json;
	//console.log(params);
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
	
	getJSONData("getRaces", "", "", "", function(data){
		addFilterElements(data);
	});	
	
	getJSONData("getClasses", "", "", "", function(data){
		addFilterElements(data);
	});
	
	getJSONData("getLocations", "", "", "", function(data){
		addFilterElements(data);
	});
	
	getJSONData("getExpansions", "", "", "", function(data){
		addFilterElements(data);
	});
	
	getJSONData("getFactions", "", "", "", function(data){
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
	const span1 = document.createElement('span');
	const span2 = document.createElement('span');
	header.id = sectionHeaderText + "HeaderId";	
	header.classList.add('btn');
	header.classList.add('filterBtn');
	header.setAttribute('section', sectionHeaderText);
	div.appendChild(header);
	
	span1.textContent = sectionHeaderText;
	span2.textContent = '+';
	span2.classList.add('PlusMinusSpan');
	header.append(span1, span2);
	
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
	const span = h.querySelector('.PlusMinusSpan');
	
	if(state==1){
		target.classList.remove('collapse');
		h.setAttribute('is-collapsed', '0');
		span.textContent = '-';
	}else{
		target.classList.add('collapse');
		h.setAttribute('is-collapsed', '1');
		span.textContent = '+';
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
	var selectValue = document.getElementById('showSelectId').value;
	var sortValue = document.getElementById('sortBySelectId').value;
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

	getJSONData("getData", selectValue, sortValue, excludes, function(data){
		console.log(data);
		buildTable(data);
		b.disabled = false;
	});
}

function buildTable(json){
	try {
		const obj = JSON.parse(json);
		
		//console.log(obj);
		
		
		//get the container
		const container = document.getElementById("ResultsDiv");
		
		//clear out any existing children
		container.innerHTML = '';
		
		const table = document.createElement('table');
		table.classList.add('border');
		table.classList.add('m-2');
		table.classList.add('content');
		container.appendChild(table);
		
		//Create header row of table using keys from first node
		
		
		const tableHeader = document.createElement('tr');
		table.appendChild(tableHeader);
		
		Object.keys(obj[0]).forEach(key => { 
			const th = document.createElement('th');
			th.textContent = key;
			
			tableHeader.appendChild(th);
			
		});
		
		for(let i = 0; i < obj.length; i++){
			const row = document.createElement('tr');
			table.appendChild(row);
			Object.keys(obj[i]).forEach(key => {
				let _value = obj[i][key];
				
				const cell = document.createElement('td');
				cell.textContent = _value;
				row.appendChild(cell);
			});
			
		}
		
		//Iterate through all data
		Object.keys(obj).forEach(key => {
			
			Object.keys(obj[key]).forEach(k => {
				
			//console.log(`Key2: ${k}, Value: ${obj[key][k]}`);
				
			});
		});
	} catch (error) {
		console.error("Error parsing JSON:", error.message);
	}
}
