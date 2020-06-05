default: build

IMG=api-svc
SLN=template
API=$(SLN).Api/$(SLN).Api.csproj
VER=v0.0.1


clean:
	dotnet clean

restore: clean
	dotnet restore --no-cache --force

build:
	dotnet build $(SLN).sln -c Debug --force --nologo

run: build
	dotnet run -p $(API)

test: build
	dotnet test /p:CollectCoverage=true

publish:
	dotnet publish -r linux-musl-x64 -c Release -o ./$(SLN).api/publish $(API)

img:
	docker build -t $(IMG):$(VER) template.api/. --no-cache