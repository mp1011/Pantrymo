ALTER TABLE Authors ADD LastModified DateTime null
go
UPDATE Authors SET LastModified = GETUTCDATE()

ALTER TABLE ComponentNegativeRelations ADD LastModified DateTime null
go
UPDATE ComponentNegativeRelations SET LastModified = GETUTCDATE()

ALTER TABLE IngredientText ADD LastModified DateTime null
go
UPDATE IngredientText SET LastModified = GETUTCDATE()

ALTER TABLE RecipeIngredients ADD LastModified DateTime null
go
UPDATE RecipeIngredients SET LastModified = GETUTCDATE()