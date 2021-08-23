create table tblLogin (
	loginId INT Primary key identity(1,1),
	userId uniqueidentifier unique foreign key references tblusers(userId),
	passwordHash varbinary(max) not null,
	passwordSalt varbinary(max) not null,
	createdBy uniqueidentifier not null,
	createdDate datetime not null,
	modifiedBy uniqueidentifier,
	modifiedDate datetime,
	isArchived bit default 0 not null
)
