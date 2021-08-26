create table tblLogin (
	loginId int Primary key identity(1,1),
	userId uniqueidentifier not null unique foreign key references tblusers(userId),
	passwordHash varbinary(max) not null,
	passwordSalt varbinary(max) not null,
	createdBy uniqueidentifier not null,
	createdDate datetime not null,
	modifiedBy uniqueidentifier,
	modifiedDate datetime,
	isArchived bit default 0 not null
)

create table tblPages (
	pageId int Primary key identity(1,1),
	pageName varchar(20) not null,
	parentPageId int null foreign key references tblPages(pageId),
	createdBy uniqueidentifier not null,
	createdDate datetime not null,
	modifiedBy uniqueidentifier,
	modifiedDate datetime,
	isTab bit not null,
	isAddEditPage bit not null,
	isExternal bit not null,
	isArchived bit default 0 not null
)

create table tblUserPages (
	userPageId int Primary key identity(1,1),
	userId uniqueidentifier not null foreign key references tblusers(userId),
	pageId int not null foreign key references tblPages(pageId),
	createdBy uniqueidentifier not null,
	createdDate datetime not null,
	modifiedBy uniqueidentifier,
	modifiedDate datetime,
	isArchived bit default 0 not null
)
create table tblRolePages (
	rolePageId int primary key identity(1,1),
	roleId int not null foreign key references tblRoles(roleId),
	pageId int not null foreign key references tblPages(pageId),
	createdBy uniqueidentifier not null,
	createdDate datetime not null,
	modifiedBy uniqueidentifier,
	modifiedDate datetime,
	isArchived bit default 0 not null,
	CONSTRAINT UC_RolePage UNIQUE (roleId,pageId)
)
