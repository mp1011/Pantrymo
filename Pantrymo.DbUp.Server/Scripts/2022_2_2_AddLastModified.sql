ALTER TABLE Recipes ADD LastModified DateTime null
go
UPDATE Recipes SET LastModified = GETUTCDATE()
ALTER TABLE Recipes ALTER COlUMN LastModified DateTime not null

ALTER TABLE RecipeSteps ADD LastModified DateTime null
go
UPDATE RecipeSteps SET LastModified = GETUTCDATE()
ALTER TABLE RecipeSteps ALTER COlUMN LastModified DateTime not null

ALTER TABLE Cuisines ADD LastModified DateTime null
go
UPDATE Cuisines SET LastModified = GETUTCDATE()
ALTER TABLE Cuisines ALTER COlUMN LastModified DateTime not null

