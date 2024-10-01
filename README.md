# API
เป็น api เข้าระบบ โดยใช้ Authorization

## Description
ระบบเข้าสู่ระบบ โดยมี token เข้ารหัส HmacSha256 
โปรเจคนี้เป็น API เข้าระบบ หลังจากเข้าระบบแล้วก็สามารถ นำไปใช้ ป้องกันการเข้าระบบ API หลังบ้านของเรา
ฐานข้อมูลที่ใช้เป็น DB SQLSERVER 

## Installation
การติดตั้งตัวโปรเจค
 1. Deploy project .net 8 
 2. IIS (Internet Information Services) run ที่ IIS

ติดตั้งฐานข้อมูล SQLSERVER 
  สร้าง ตาราง User
   id 
   userName
   password
