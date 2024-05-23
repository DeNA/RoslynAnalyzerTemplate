.PHONY: help
help: ## Display this help screen
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'	

.PHONY: all
all: build copy ## Build and Copy

.PHONY: build
build: ## Build csproj
	rm -rf output
	dotnet build --no-incremental RoslynAnalyzerTemplate/RoslynAnalyzerTemplate.csproj --output output --configuration Release

.PHONY: copy
copy: ## Copy dll file to Unity Project
	rm -f RoslynAnalyzerTemplate.Unity/Assets/Plugins/RoslynAnalyzerTemplate/RoslynAnalyzerTemplate.dll*
	cp output/RoslynAnalyzerTemplate.dll RoslynAnalyzerTemplate.Unity/Assets/Plugins/RoslynAnalyzerTemplate/RoslynAnalyzerTemplate.dll
