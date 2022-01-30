ALTER TABLE Components ADD LastModified DateTime null
go
UPDATE Components SET LastModified = GETUTCDATE()
ALTER TABLE Components ALTER COlUMN LastModified DateTime not null

ALTER TABLE AlternateComponentNames ADD LastModified DateTime null
go
UPDATE AlternateComponentNames SET LastModified = GETUTCDATE()
ALTER TABLE AlternateComponentNames ALTER COlUMN LastModified DateTime not null

ALTER TABLE ComponentHierarchy ADD LastModified DateTime null
go
UPDATE ComponentHierarchy SET LastModified = GETUTCDATE()
ALTER TABLE ComponentHierarchy ALTER COlUMN LastModified DateTime not null