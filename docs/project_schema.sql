
    PRAGMA foreign_keys = OFF

    drop table if exists COMMANDS

    drop table if exists COMMAND_PROPERTIES

    drop table if exists DATA_REPOSITORIES

    drop table if exists ENTITIES

    drop table if exists SIMULATIONS

    drop table if exists HISTORY_ITEMS

    drop table if exists CONTENTS

    drop table if exists PROJECTS

    PRAGMA foreign_keys = ON

    create table COMMANDS (
        Id TEXT not null,
       CommandId TEXT not null,
       Discriminator TEXT not null,
       CommandInverseId TEXT,
       CommandType TEXT,
       ObjectType TEXT,
       Description TEXT,
       ExtendedDescription TEXT,
       Visible INTEGER,
       Comment TEXT,
       Sequence INTEGER,
       ParentId TEXT,
       primary key (Id),
       constraint FK_DD4B87FE foreign key (ParentId) references COMMANDS
    )

    create table COMMAND_PROPERTIES (
        Id  integer primary key autoincrement,
       Name TEXT,
       Value TEXT,
       CommandId TEXT,
       constraint FK_71C9276E foreign key (CommandId) references COMMANDS
    )

    create table DATA_REPOSITORIES (
        Id TEXT not null,
       ContentId INTEGER,
       SimulationId TEXT,
       primary key (Id),
       constraint fk_DataRepository_Content foreign key (ContentId) references CONTENTS,
       constraint FK_524447BC foreign key (SimulationId) references SIMULATIONS
    )

    create table ENTITIES (
        Id TEXT not null,
       Discriminator TEXT not null,
       ContentId INTEGER,
       ProjectId INTEGER,
       primary key (Id),
       constraint fk_Entity_Content foreign key (ContentId) references CONTENTS,
       constraint FK_9F0A793C foreign key (ProjectId) references PROJECTS
    )

    create table SIMULATIONS (
        SimulationId TEXT not null,
       primary key (SimulationId),
       constraint FK_A2608560 foreign key (SimulationId) references ENTITIES
    )

    create table HISTORY_ITEMS (
        Id TEXT not null,
       UserId TEXT,
       DateTime TEXT,
       State INTEGER,
       Sequence INTEGER,
       CommandId TEXT,
       primary key (Id),
       constraint fk_HistoryItem_Command foreign key (CommandId) references COMMANDS
    )

    create table CONTENTS (
        Id  integer primary key autoincrement,
       Data image
    )

    create table PROJECTS (
        Id  integer primary key autoincrement,
       Version INTEGER,
       ContentId INTEGER,
       constraint fk_Project_Content foreign key (ContentId) references CONTENTS
    )
