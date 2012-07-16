<?php

	
function wikiplugin_localfilelink_help() {
	return tra("Used to link to local files and directories. Opens Windows Explorer and selects file or directory. Only works with Internet Explorer.").":<br />~np~{LOCALFILELINK(link=>)}".tra("text")."{LOCALFILELINK}~/np~ ";
}

function wikiplugin_localfilelink_info() {
	return array(
		'name' => tra('Local File Link'),
		'documentation' => tra('PluginLocalFileLink'),
		'description' => tra('Used to link to local files and directories without downloading them.'),
		'prefs' => array('wikiplugin_localfilelink'),
		'body' => tra('Text to display.'),
		'icon' => 'img/icons/file-manager.png',
		'params' => array(
			'link' => array(
				'required' => true,
				'name' => tra('Path'),
				'description' => tra('Full path of file or directory.'),
				'default' => ''
			)
		)
	);
}

function wikiplugin_localfilelink($data, $params) {

	//create unique id number
	static $iLocalFileLink = 0;
	$iLocalFileLink++;
	
	extract ($params,EXTR_SKIP);
	$link = str_replace('\\', '\\\\', $link);
		
  $wret  = '<span id="localfilelink_' . $iLocalFileLink . '">';
	$wret .= '<script type="text/javascript" src="lib/wiki-plugins/wikiplugin_localfilelink.js">&nbsp;</script>';
	$wret .= '<a href="javascript:tikiLocalFileLink.openFileFolder(\'' . $link . '\')">' . $data . '</a>';
	$wret .= '</span>';
	
	return $wret;
}