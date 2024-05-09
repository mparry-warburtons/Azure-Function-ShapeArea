
--drop  table example.RawShapes

create table example.Shapes
(
	Id int identity(1,1),
	ShapeName nvarchar(50) not null,
	Dimensions nvarchar(50) null default 'N/A',
	Units nvarchar(50) not null,
	Radius decimal(5,3) default 0,
	Area decimal(10,5)
)


insert into example.Shapes
( ShapeName,Dimensions,Units,Radius, Area)

select 'Rectangle' , '5x5' , 'cm' , 0 , null  union all 
select 'Triangle' , '5x4x3' , 'cm' , 0  , null    union all 
select 'Circle' , 'N/A' , 'cm' , 4.4 , null 


select * from example.Shapes