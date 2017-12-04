// Archivo JScript

function celdaSobre(__obj, __estado){
	if (__estado)
		__obj.addClassName('mouseSobreCelda');
	else
		__obj.removeClassName('mouseSobreCelda');
}

function ocultar(__obj, __estado){
	if (__estado)
		__obj.addClassName('visible');
	else
		__obj.removeClassName('invisible');
}

function abreResponsable() {
	ancho=650;
	alto=400;
	LeftPosition=(screen.width)?(screen.width-ancho)/2:100;
	TopPosition=(screen.height)?(screen.height-alto)/2:100;
	settings='width='+ancho+',height='+alto+',top='+TopPosition+',left='+LeftPosition+',scrollbars=YES,status=NO,location=NO,directories=NO,menubar=NO,toolbar=NO,resizable=NO';
	win = window.open("wResponsable.aspx","::NAEX:: Soluciones Informáticas",settings);
}
