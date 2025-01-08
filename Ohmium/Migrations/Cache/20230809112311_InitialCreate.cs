using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ohmium.Migrations.Cache
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "colorConfig",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sensorName = table.Column<string>(type: "TEXT", nullable: true),
                    colorCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colorConfig", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "deviceDataLog",
                columns: table => new
                {
                    timeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deviceDataLog", x => x.timeStamp);
                });

            migrationBuilder.CreateTable(
                name: "equipmentConfiguration",
                columns: table => new
                {
                    equipmentConfigID = table.Column<Guid>(type: "TEXT", nullable: false),
                    configName = table.Column<string>(type: "TEXT", nullable: true),
                    equipmentConfiguration = table.Column<string>(type: "TEXT", nullable: true),
                    createdOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    createdBy = table.Column<string>(type: "TEXT", nullable: true),
                    updatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    colorConfig = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipmentConfiguration", x => x.equipmentConfigID);
                });

            migrationBuilder.CreateTable(
                name: "mtsDeviceData",
                columns: table => new
                {
                    deviceID = table.Column<string>(type: "TEXT", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    siteID = table.Column<string>(type: "TEXT", nullable: false),
                    configID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ver = table.Column<string>(type: "TEXT", nullable: true),
                    status = table.Column<int>(type: "INTEGER", nullable: false),
                    wL = table.Column<float>(type: "REAL", nullable: true),
                    wP = table.Column<float>(type: "REAL", nullable: true),
                    wC = table.Column<float>(type: "REAL", nullable: true),
                    wT = table.Column<float>(type: "REAL", nullable: true),
                    hxiT = table.Column<float>(type: "REAL", nullable: true),
                    hxoT = table.Column<float>(type: "REAL", nullable: true),
                    HYS = table.Column<float>(type: "REAL", nullable: true),
                    wPp = table.Column<float>(type: "REAL", nullable: true),
                    verM = table.Column<string>(type: "TEXT", nullable: true),
                    CommStatus = table.Column<short>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtsDeviceData", x => new { x.deviceID, x.timeStamp });
                });

            migrationBuilder.CreateTable(
                name: "mtsStackData",
                columns: table => new
                {
                    deviceID = table.Column<string>(type: "TEXT", nullable: false),
                    stackMfgID = table.Column<string>(type: "TEXT", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    position = table.Column<string>(type: "TEXT", nullable: true),
                    wF = table.Column<float>(type: "REAL", nullable: true),
                    wT = table.Column<float>(type: "REAL", nullable: true),
                    wP = table.Column<float>(type: "REAL", nullable: true),
                    hT = table.Column<float>(type: "REAL", nullable: true),
                    hP = table.Column<float>(type: "REAL", nullable: true),
                    psI = table.Column<float>(type: "REAL", nullable: true),
                    psV = table.Column<float>(type: "REAL", nullable: true),
                    cV1 = table.Column<float>(type: "REAL", nullable: true),
                    cV2 = table.Column<float>(type: "REAL", nullable: true),
                    cV3 = table.Column<float>(type: "REAL", nullable: true),
                    cV4 = table.Column<float>(type: "REAL", nullable: true),
                    cV5 = table.Column<float>(type: "REAL", nullable: true),
                    isF = table.Column<float>(type: "REAL", nullable: true),
                    cV6 = table.Column<float>(type: "REAL", nullable: true),
                    cR1 = table.Column<float>(type: "REAL", nullable: true),
                    cR2 = table.Column<float>(type: "REAL", nullable: true),
                    cR3 = table.Column<float>(type: "REAL", nullable: true),
                    cR4 = table.Column<float>(type: "REAL", nullable: true),
                    cR5 = table.Column<float>(type: "REAL", nullable: true),
                    cX1 = table.Column<float>(type: "REAL", nullable: true),
                    cX2 = table.Column<float>(type: "REAL", nullable: true),
                    cX3 = table.Column<float>(type: "REAL", nullable: true),
                    cX4 = table.Column<float>(type: "REAL", nullable: true),
                    cX5 = table.Column<float>(type: "REAL", nullable: true),
                    cR6 = table.Column<float>(type: "REAL", nullable: true),
                    CD = table.Column<float>(type: "REAL", nullable: true),
                    state = table.Column<string>(type: "TEXT", nullable: true),
                    runHours = table.Column<float>(type: "REAL", nullable: true),
                    cumulativeHours = table.Column<float>(type: "REAL", nullable: true),
                    stepNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    loopcnt = table.Column<int>(type: "INTEGER", nullable: true),
                    cM1 = table.Column<float>(type: "REAL", nullable: true),
                    cM2 = table.Column<float>(type: "REAL", nullable: true),
                    cM3 = table.Column<float>(type: "REAL", nullable: true),
                    cM4 = table.Column<float>(type: "REAL", nullable: true),
                    cM5 = table.Column<float>(type: "REAL", nullable: true),
                    seqName = table.Column<string>(type: "TEXT", nullable: true),
                    imF = table.Column<float>(type: "REAL", nullable: true),
                    imA = table.Column<float>(type: "REAL", nullable: true),
                    status = table.Column<int>(type: "INTEGER", nullable: true),
                    interLock = table.Column<string>(type: "TEXT", nullable: true),
                    scriptLoopCnt = table.Column<float>(type: "REAL", nullable: true),
                    seqLoopCnt = table.Column<float>(type: "REAL", nullable: true),
                    scriptStep = table.Column<float>(type: "REAL", nullable: true),
                    seqStep = table.Column<float>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtsStackData", x => new { x.deviceID, x.stackMfgID, x.timeStamp });
                });

            migrationBuilder.CreateTable(
                name: "org",
                columns: table => new
                {
                    OrgID = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrgName = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: true),
                    createdOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    createdBy = table.Column<string>(type: "TEXT", nullable: true),
                    updatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_org", x => x.OrgID);
                });

            migrationBuilder.CreateTable(
                name: "orgCache",
                columns: table => new
                {
                    OrgID = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrgName = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: true),
                    createdOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    createdBy = table.Column<string>(type: "TEXT", nullable: true),
                    updatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orgCache", x => x.OrgID);
                });

            migrationBuilder.CreateTable(
                name: "region",
                columns: table => new
                {
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    desc = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_region", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "sconfig",
                columns: table => new
                {
                    configID = table.Column<Guid>(type: "TEXT", nullable: false),
                    configName = table.Column<string>(type: "TEXT", nullable: true),
                    configString = table.Column<string>(type: "TEXT", nullable: true),
                    colorConfig = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sconfig", x => x.configID);
                });

            migrationBuilder.CreateTable(
                name: "segment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    type = table.Column<string>(type: "TEXT", nullable: true),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_segment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SequenceLibrary",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sequenceName = table.Column<string>(type: "TEXT", nullable: true),
                    loopCount = table.Column<int>(type: "INTEGER", nullable: true),
                    sortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceLibrary", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "statusType",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "testProfileConfig",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Config = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testProfileConfig", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    sqlId = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: true),
                    orgID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: true),
                    siteLat = table.Column<float>(type: "REAL", nullable: false),
                    siteLng = table.Column<float>(type: "REAL", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: true),
                    h2Production = table.Column<float>(type: "REAL", nullable: false),
                    powerConsumption = table.Column<float>(type: "REAL", nullable: false),
                    siteEfficiency = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site", x => x.id);
                    table.ForeignKey(
                        name: "FK_site_org_orgID",
                        column: x => x.orgID,
                        principalTable: "org",
                        principalColumn: "OrgID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_site_region_Region",
                        column: x => x.Region,
                        principalTable: "region",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateTable(
                name: "RunStepLibrary",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    seqMasterId = table.Column<int>(type: "INTEGER", nullable: true),
                    stepNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    duration = table.Column<int>(type: "INTEGER", nullable: false),
                    cI = table.Column<float>(type: "REAL", nullable: true),
                    cV = table.Column<float>(type: "REAL", nullable: true),
                    wP = table.Column<float>(type: "REAL", nullable: true),
                    hP = table.Column<float>(type: "REAL", nullable: true),
                    wFt = table.Column<float>(type: "REAL", nullable: true),
                    wTt = table.Column<float>(type: "REAL", nullable: true),
                    cVt = table.Column<float>(type: "REAL", nullable: true),
                    cVlimit = table.Column<float>(type: "REAL", nullable: true),
                    mnF = table.Column<float>(type: "REAL", nullable: true),
                    mxF = table.Column<float>(type: "REAL", nullable: true),
                    imF = table.Column<float>(type: "REAL", nullable: true),
                    imA = table.Column<float>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunStepLibrary", x => x.id);
                    table.ForeignKey(
                        name: "FK_RunStepLibrary_SequenceLibrary_seqMasterId",
                        column: x => x.seqMasterId,
                        principalTable: "SequenceLibrary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    EqMfgID = table.Column<string>(type: "TEXT", nullable: false),
                    EqDes = table.Column<string>(type: "TEXT", nullable: true),
                    deviceType = table.Column<string>(type: "TEXT", nullable: true),
                    siteID = table.Column<Guid>(type: "TEXT", nullable: false),
                    configID = table.Column<Guid>(type: "TEXT", nullable: false),
                    comStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    h2Production = table.Column<float>(type: "REAL", nullable: false),
                    powerConsumption = table.Column<float>(type: "REAL", nullable: false),
                    siteEfficiency = table.Column<float>(type: "REAL", nullable: false),
                    createdOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    createdBy = table.Column<string>(type: "TEXT", nullable: true),
                    updatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    nStack = table.Column<int>(type: "INTEGER", nullable: false),
                    status = table.Column<int>(type: "INTEGER", nullable: true),
                    ver = table.Column<string>(type: "TEXT", nullable: true),
                    isRunning = table.Column<string>(type: "TEXT", nullable: true),
                    lastDataReceivedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    wL = table.Column<float>(type: "REAL", nullable: true),
                    wP = table.Column<float>(type: "REAL", nullable: true),
                    wC = table.Column<float>(type: "REAL", nullable: true),
                    wT = table.Column<float>(type: "REAL", nullable: true),
                    hxiT = table.Column<float>(type: "REAL", nullable: true),
                    hxoT = table.Column<float>(type: "REAL", nullable: true),
                    HYS = table.Column<float>(type: "REAL", nullable: true),
                    wPp = table.Column<float>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device", x => x.EqMfgID);
                    table.ForeignKey(
                        name: "FK_device_equipmentConfiguration_configID",
                        column: x => x.configID,
                        principalTable: "equipmentConfiguration",
                        principalColumn: "equipmentConfigID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_device_site_siteID",
                        column: x => x.siteID,
                        principalTable: "site",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_device_statusType_status",
                        column: x => x.status,
                        principalTable: "statusType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    sid = table.Column<Guid>(type: "TEXT", nullable: false),
                    address1 = table.Column<string>(type: "TEXT", nullable: false),
                    address2 = table.Column<string>(type: "TEXT", nullable: true),
                    address3 = table.Column<string>(type: "TEXT", nullable: true),
                    postalCode = table.Column<string>(type: "TEXT", nullable: false),
                    city = table.Column<string>(type: "TEXT", nullable: false),
                    state = table.Column<string>(type: "TEXT", nullable: false),
                    country = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_address_Site_sid",
                        column: x => x.sid,
                        principalTable: "Site",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    siteID = table.Column<Guid>(type: "TEXT", nullable: false),
                    contactName = table.Column<string>(type: "TEXT", nullable: true),
                    phone = table.Column<string>(type: "TEXT", nullable: true),
                    email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.id);
                    table.ForeignKey(
                        name: "FK_Contact_Site_siteID",
                        column: x => x.siteID,
                        principalTable: "Site",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stack",
                columns: table => new
                {
                    stackMfgID = table.Column<string>(type: "TEXT", nullable: false),
                    siteID = table.Column<Guid>(type: "TEXT", nullable: false),
                    deviceID = table.Column<string>(type: "TEXT", nullable: true),
                    stackConfig = table.Column<Guid>(type: "TEXT", nullable: false),
                    stackPosition = table.Column<string>(type: "TEXT", nullable: true),
                    meaNum = table.Column<int>(type: "INTEGER", nullable: false),
                    meaArea = table.Column<float>(type: "REAL", nullable: false),
                    status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stack", x => x.stackMfgID);
                    table.ForeignKey(
                        name: "FK_stack_device_deviceID",
                        column: x => x.deviceID,
                        principalTable: "device",
                        principalColumn: "EqMfgID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stack_sconfig_stackConfig",
                        column: x => x.stackConfig,
                        principalTable: "sconfig",
                        principalColumn: "configID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stack_site_siteID",
                        column: x => x.siteID,
                        principalTable: "site",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stack_statusType_status",
                        column: x => x.status,
                        principalTable: "statusType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_sid",
                table: "address",
                column: "sid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contact_siteID",
                table: "Contact",
                column: "siteID");

            migrationBuilder.CreateIndex(
                name: "IX_device_configID",
                table: "device",
                column: "configID");

            migrationBuilder.CreateIndex(
                name: "IX_device_siteID",
                table: "device",
                column: "siteID");

            migrationBuilder.CreateIndex(
                name: "IX_device_status",
                table: "device",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_RunStepLibrary_seqMasterId",
                table: "RunStepLibrary",
                column: "seqMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_site_orgID",
                table: "site",
                column: "orgID");

            migrationBuilder.CreateIndex(
                name: "IX_site_Region",
                table: "site",
                column: "Region");


            migrationBuilder.CreateIndex(
                name: "IX_stack_deviceID",
                table: "stack",
                column: "deviceID");

            migrationBuilder.CreateIndex(
                name: "IX_stack_siteID",
                table: "stack",
                column: "siteID");

            migrationBuilder.CreateIndex(
                name: "IX_stack_stackConfig",
                table: "stack",
                column: "stackConfig");

            migrationBuilder.CreateIndex(
                name: "IX_stack_status",
                table: "stack",
                column: "status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "colorConfig");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "deviceDataLog");

            migrationBuilder.DropTable(
                name: "mtsDeviceData");

            migrationBuilder.DropTable(
                name: "mtsStackData");

            migrationBuilder.DropTable(
                name: "orgCache");

            migrationBuilder.DropTable(
                name: "RunStepLibrary");

            migrationBuilder.DropTable(
                name: "segment");

            migrationBuilder.DropTable(
                name: "stack");

            migrationBuilder.DropTable(
                name: "testProfileConfig");

            migrationBuilder.DropTable(
                name: "Site");

            migrationBuilder.DropTable(
                name: "SequenceLibrary");

            migrationBuilder.DropTable(
                name: "device");

            migrationBuilder.DropTable(
                name: "sconfig");

            migrationBuilder.DropTable(
                name: "equipmentConfiguration");

            migrationBuilder.DropTable(
                name: "site");

            migrationBuilder.DropTable(
                name: "statusType");

            migrationBuilder.DropTable(
                name: "org");

            migrationBuilder.DropTable(
                name: "region");
        }
    }
}
