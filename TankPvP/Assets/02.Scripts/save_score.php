<?php
	//�����ͺ��̽� ����
	$connect = mysql_connect("localhost", "u101113475_hoon", "jung1920");

	//�����ͺ��̽� ���� �������� Ȯ��
	if($connect == 0)
		{
			?>SORRY SCORE SERVER CONNECTION ERRER<?
		}
		else
		{
		}

	//����� �����ͺ��̽� ����
	mysql_select_db("u101113475_jankee", $connect);

	//post������� ���޵� �Ķ���͸� ������ ����
	$user_name = $_POST["user_name"];
	$kill_count = $_POST["kill_count"];
	$seq_no = $_POST["seq_no"];

	//SQL Query����
	$sql = "INSERT INTO tb_score(user_name, kill_count, seq_no)";
	$sql = "\n VALUES ('".$user_name."', ".$kill_count.", ".$seq_no.")";
	$sql = "\n ON DUPLICATE KEY UPDATE kill_count = kill_count + 1";

	//SQL Query ����
	$result = mysql_query($sql, $connect);

	//Query ���� ��� ��ȯ
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