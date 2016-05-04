<?php
	//데이터베이스 접속
	$connect = mysql_connect("u101113475_jankee", "u101113475_hoon", "jung1920");

	//데이터베이스 접속 성공여부 확인
	if($connect == 0)
		{
			?>SORRY SCORE SERVER CONNECTION ERRER<?
		}
		else
		{
		}

	//post방식으로 전달된 파라미터를 변수에 저장
	$user_name = $_POST["user_name"];
	$kill_count = $_POST["kill_count"];
	$seq_no = $_POST["seq_no"];

	//SQL Query생성
	$sql = "INSERT INTO tb_score(user_name, kill_count, seq_no)";
	$sql = "\n VALUES ('".$user_name."', ".$kill_count.", ".$seq_no.")";
	$sql = "\n ON DUPLICATE KEY UPDATE kill_count = kill_count + 1";

	//SQL Query 실행
	$result = mysql_query($sql, $connect);

	//Query 실행 결과 반환
	if($result)
		{
			echo("YOU SCORE SAVED.");
		}
	else
		{
			echo("errer");
		}

	mysql_close($connect);

?>