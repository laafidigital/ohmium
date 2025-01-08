using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ohmium.Repository.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "colorConfig",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sensorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    colorCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colorConfig", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "equipmentConfiguration",
                columns: table => new
                {
                    equipmentConfigID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    configName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    equipmentConfiguration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    colorConfig = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipmentConfiguration", x => x.equipmentConfigID);
                });

            migrationBuilder.CreateTable(
                name: "feedback",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dstamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lotusTempdata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lotusTempdata", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mmp",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    equipment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sensorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    min = table.Column<float>(type: "real", nullable: false),
                    max = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mmp", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mtsDeviceData",
                columns: table => new
                {
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    siteID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    configID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    wL = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    wC = table.Column<float>(type: "real", nullable: true),
                    wT = table.Column<float>(type: "real", nullable: true),
                    hxiT = table.Column<float>(type: "real", nullable: true),
                    hxoT = table.Column<float>(type: "real", nullable: true),
                    HYS = table.Column<float>(type: "real", nullable: true),
                    wPp = table.Column<float>(type: "real", nullable: true),
                    verM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommStatus = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtsDeviceData", x => new { x.deviceID, x.timeStamp });
                });

            migrationBuilder.CreateTable(
                name: "mtsDeviceDataNew",
                columns: table => new
                {
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    siteID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    configID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    wL = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    wC = table.Column<float>(type: "real", nullable: true),
                    wT = table.Column<float>(type: "real", nullable: true),
                    hxiT = table.Column<float>(type: "real", nullable: true),
                    hxoT = table.Column<float>(type: "real", nullable: true),
                    HYS = table.Column<float>(type: "real", nullable: true),
                    wPp = table.Column<float>(type: "real", nullable: true),
                    verM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommStatus = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtsDeviceDataNew", x => new { x.deviceID, x.timeStamp });
                });

            migrationBuilder.CreateTable(
                name: "mtsDeviceDataNew2",
                columns: table => new
                {
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    siteID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    configID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    wL = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    wC = table.Column<float>(type: "real", nullable: true),
                    wT = table.Column<float>(type: "real", nullable: true),
                    hxiT = table.Column<float>(type: "real", nullable: true),
                    hxoT = table.Column<float>(type: "real", nullable: true),
                    HYS = table.Column<float>(type: "real", nullable: true),
                    wPp = table.Column<float>(type: "real", nullable: true),
                    verM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommStatus = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtsDeviceDataNew2", x => new { x.deviceID, x.timeStamp });
                });

            migrationBuilder.CreateTable(
                name: "mtsStackData",
                columns: table => new
                {
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    stackMfgID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    wF = table.Column<float>(type: "real", nullable: true),
                    wT = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    hT = table.Column<float>(type: "real", nullable: true),
                    hP = table.Column<float>(type: "real", nullable: true),
                    psI = table.Column<float>(type: "real", nullable: true),
                    psV = table.Column<float>(type: "real", nullable: true),
                    cV1 = table.Column<float>(type: "real", nullable: true),
                    cV2 = table.Column<float>(type: "real", nullable: true),
                    cV3 = table.Column<float>(type: "real", nullable: true),
                    cV4 = table.Column<float>(type: "real", nullable: true),
                    cV5 = table.Column<float>(type: "real", nullable: true),
                    isF = table.Column<float>(type: "real", nullable: true),
                    cV6 = table.Column<float>(type: "real", nullable: true),
                    cR1 = table.Column<float>(type: "real", nullable: true),
                    cR2 = table.Column<float>(type: "real", nullable: true),
                    cR3 = table.Column<float>(type: "real", nullable: true),
                    cR4 = table.Column<float>(type: "real", nullable: true),
                    cR5 = table.Column<float>(type: "real", nullable: true),
                    cX1 = table.Column<float>(type: "real", nullable: true),
                    cX2 = table.Column<float>(type: "real", nullable: true),
                    cX3 = table.Column<float>(type: "real", nullable: true),
                    cX4 = table.Column<float>(type: "real", nullable: true),
                    cX5 = table.Column<float>(type: "real", nullable: true),
                    cR6 = table.Column<float>(type: "real", nullable: true),
                    CD = table.Column<float>(type: "real", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    runHours = table.Column<float>(type: "real", nullable: true),
                    cumulativeHours = table.Column<float>(type: "real", nullable: true),
                    stepNumber = table.Column<int>(type: "int", nullable: true),
                    loopcnt = table.Column<int>(type: "int", nullable: true),
                    cM1 = table.Column<float>(type: "real", nullable: true),
                    cM2 = table.Column<float>(type: "real", nullable: true),
                    cM3 = table.Column<float>(type: "real", nullable: true),
                    cM4 = table.Column<float>(type: "real", nullable: true),
                    cM5 = table.Column<float>(type: "real", nullable: true),
                    seqName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    imF = table.Column<float>(type: "real", nullable: true),
                    imA = table.Column<float>(type: "real", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    interLock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scriptLoopCnt = table.Column<float>(type: "real", nullable: true),
                    seqLoopCnt = table.Column<float>(type: "real", nullable: true),
                    scriptStep = table.Column<float>(type: "real", nullable: true),
                    seqStep = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtsStackData", x => new { x.deviceID, x.stackMfgID, x.timeStamp });
                });

            migrationBuilder.CreateTable(
                name: "mtsStackDataNew",
                columns: table => new
                {
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    stackMfgID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    wF = table.Column<float>(type: "real", nullable: true),
                    wT = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    hT = table.Column<float>(type: "real", nullable: true),
                    hP = table.Column<float>(type: "real", nullable: true),
                    psI = table.Column<float>(type: "real", nullable: true),
                    psV = table.Column<float>(type: "real", nullable: true),
                    cV1 = table.Column<float>(type: "real", nullable: true),
                    cV2 = table.Column<float>(type: "real", nullable: true),
                    cV3 = table.Column<float>(type: "real", nullable: true),
                    cV4 = table.Column<float>(type: "real", nullable: true),
                    cV5 = table.Column<float>(type: "real", nullable: true),
                    isF = table.Column<float>(type: "real", nullable: true),
                    cV6 = table.Column<float>(type: "real", nullable: true),
                    cR1 = table.Column<float>(type: "real", nullable: true),
                    cR2 = table.Column<float>(type: "real", nullable: true),
                    cR3 = table.Column<float>(type: "real", nullable: true),
                    cR4 = table.Column<float>(type: "real", nullable: true),
                    cR5 = table.Column<float>(type: "real", nullable: true),
                    cX1 = table.Column<float>(type: "real", nullable: true),
                    cX2 = table.Column<float>(type: "real", nullable: true),
                    cX3 = table.Column<float>(type: "real", nullable: true),
                    cX4 = table.Column<float>(type: "real", nullable: true),
                    cX5 = table.Column<float>(type: "real", nullable: true),
                    cR6 = table.Column<float>(type: "real", nullable: true),
                    CD = table.Column<float>(type: "real", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    runHours = table.Column<float>(type: "real", nullable: true),
                    cumulativeHours = table.Column<float>(type: "real", nullable: true),
                    stepNumber = table.Column<int>(type: "int", nullable: true),
                    loopcnt = table.Column<int>(type: "int", nullable: true),
                    cM1 = table.Column<float>(type: "real", nullable: true),
                    cM2 = table.Column<float>(type: "real", nullable: true),
                    cM3 = table.Column<float>(type: "real", nullable: true),
                    cM4 = table.Column<float>(type: "real", nullable: true),
                    cM5 = table.Column<float>(type: "real", nullable: true),
                    seqName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    imF = table.Column<float>(type: "real", nullable: true),
                    imA = table.Column<float>(type: "real", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    interLock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scriptLoopCnt = table.Column<float>(type: "real", nullable: true),
                    seqLoopCnt = table.Column<float>(type: "real", nullable: true),
                    scriptStep = table.Column<float>(type: "real", nullable: true),
                    seqStep = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtsStackDataNew", x => new { x.deviceID, x.stackMfgID, x.timeStamp });
                });

            migrationBuilder.CreateTable(
                name: "mtsStackDataNew2",
                columns: table => new
                {
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    stackMfgID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    wF = table.Column<float>(type: "real", nullable: true),
                    wT = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    hT = table.Column<float>(type: "real", nullable: true),
                    hP = table.Column<float>(type: "real", nullable: true),
                    psI = table.Column<float>(type: "real", nullable: true),
                    psV = table.Column<float>(type: "real", nullable: true),
                    cV1 = table.Column<float>(type: "real", nullable: true),
                    cV2 = table.Column<float>(type: "real", nullable: true),
                    cV3 = table.Column<float>(type: "real", nullable: true),
                    cV4 = table.Column<float>(type: "real", nullable: true),
                    cV5 = table.Column<float>(type: "real", nullable: true),
                    isF = table.Column<float>(type: "real", nullable: true),
                    cV6 = table.Column<float>(type: "real", nullable: true),
                    cR1 = table.Column<float>(type: "real", nullable: true),
                    cR2 = table.Column<float>(type: "real", nullable: true),
                    cR3 = table.Column<float>(type: "real", nullable: true),
                    cR4 = table.Column<float>(type: "real", nullable: true),
                    cR5 = table.Column<float>(type: "real", nullable: true),
                    cX1 = table.Column<float>(type: "real", nullable: true),
                    cX2 = table.Column<float>(type: "real", nullable: true),
                    cX3 = table.Column<float>(type: "real", nullable: true),
                    cX4 = table.Column<float>(type: "real", nullable: true),
                    cX5 = table.Column<float>(type: "real", nullable: true),
                    cR6 = table.Column<float>(type: "real", nullable: true),
                    CD = table.Column<float>(type: "real", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    runHours = table.Column<float>(type: "real", nullable: true),
                    cumulativeHours = table.Column<float>(type: "real", nullable: true),
                    stepNumber = table.Column<int>(type: "int", nullable: true),
                    loopcnt = table.Column<int>(type: "int", nullable: true),
                    cM1 = table.Column<float>(type: "real", nullable: true),
                    cM2 = table.Column<float>(type: "real", nullable: true),
                    cM3 = table.Column<float>(type: "real", nullable: true),
                    cM4 = table.Column<float>(type: "real", nullable: true),
                    cM5 = table.Column<float>(type: "real", nullable: true),
                    seqName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    imF = table.Column<float>(type: "real", nullable: true),
                    imA = table.Column<float>(type: "real", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    interLock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scriptLoopCnt = table.Column<float>(type: "real", nullable: true),
                    seqLoopCnt = table.Column<float>(type: "real", nullable: true),
                    scriptStep = table.Column<float>(type: "real", nullable: true),
                    seqStep = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtsStackDataNew2", x => new { x.deviceID, x.stackMfgID, x.timeStamp });
                });

            migrationBuilder.CreateTable(
                name: "org",
                columns: table => new
                {
                    OrgID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_org", x => x.OrgID);
                });

            migrationBuilder.CreateTable(
                name: "region",
                columns: table => new
                {
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_region", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "sconfig",
                columns: table => new
                {
                    configID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    configName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    configString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    colorConfig = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sconfig", x => x.configID);
                });

            migrationBuilder.CreateTable(
                name: "scriptlists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    scriptName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scriptlists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "segment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_segment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sequencyLibrary",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sequenceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    loopCount = table.Column<int>(type: "int", nullable: true),
                    sortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sequencyLibrary", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StacksThatRan",
                columns: table => new
                {
                    stackID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StacksThatRan", x => new { x.stackID, x.deviceID });
                });

            migrationBuilder.CreateTable(
                name: "stackTestRunHours",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stkMfgId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    timeStampUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cumulativeHours = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stackTestRunHours", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "statusType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sysMaster",
                columns: table => new
                {
                    scSn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    scHn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scFn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nLCC = table.Column<int>(type: "int", nullable: false),
                    nStack = table.Column<int>(type: "int", nullable: false),
                    dSn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lhc1Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lpc1Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lhc2Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lpc2Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lhc3Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lpc3Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lhc4Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lpc4Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lwcSn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ltcSn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    compSn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dryerSn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc1Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc1Hn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc1Fn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc2Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc2Hn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc2Fn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc3Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc3Hn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc3Fn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc4Sn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc4Hn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uc4Fn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sysMaster", x => x.scSn);
                });

            migrationBuilder.CreateTable(
                name: "testProfileConfig",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Config = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testProfileConfig", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "testStates",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testStates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "thresholdconfigs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    paramName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    minVal = table.Column<float>(type: "real", nullable: false),
                    maxVal = table.Column<float>(type: "real", nullable: false),
                    colorSortOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thresholdconfigs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userLogin",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    loginDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userLogin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    orgID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    siteLat = table.Column<float>(type: "real", nullable: false),
                    siteLng = table.Column<float>(type: "real", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    h2Production = table.Column<float>(type: "real", nullable: false),
                    powerConsumption = table.Column<float>(type: "real", nullable: false),
                    siteEfficiency = table.Column<float>(type: "real", nullable: false)
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
                        principalColumn: "name");
                });

            migrationBuilder.CreateTable(
                name: "stackSyncData",
                columns: table => new
                {
                    stackID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    scriptID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stackSyncData", x => new { x.stackID, x.scriptID });
                    table.ForeignKey(
                        name: "FK_stackSyncData_scriptlists_scriptID",
                        column: x => x.scriptID,
                        principalTable: "scriptlists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "runstepLibrary",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    seqMasterId = table.Column<int>(type: "int", nullable: true),
                    stepNumber = table.Column<int>(type: "int", nullable: false),
                    duration = table.Column<int>(type: "int", nullable: false),
                    cI = table.Column<float>(type: "real", nullable: true),
                    cV = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    hP = table.Column<float>(type: "real", nullable: true),
                    wFt = table.Column<float>(type: "real", nullable: true),
                    wTt = table.Column<float>(type: "real", nullable: true),
                    cVt = table.Column<float>(type: "real", nullable: true),
                    cVlimit = table.Column<float>(type: "real", nullable: true),
                    mnF = table.Column<float>(type: "real", nullable: true),
                    mxF = table.Column<float>(type: "real", nullable: true),
                    imF = table.Column<float>(type: "real", nullable: true),
                    imA = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_runstepLibrary", x => x.id);
                    table.ForeignKey(
                        name: "FK_runstepLibrary_sequencyLibrary_seqMasterId",
                        column: x => x.seqMasterId,
                        principalTable: "sequencyLibrary",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "runProfileTemplate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    profileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dCmd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fan = table.Column<bool>(type: "bit", nullable: true),
                    pump = table.Column<bool>(type: "bit", nullable: true),
                    mnHxT = table.Column<float>(type: "real", nullable: true),
                    mxHxT = table.Column<float>(type: "real", nullable: true),
                    wbT = table.Column<float>(type: "real", nullable: true),
                    mnWbT = table.Column<float>(type: "real", nullable: true),
                    mxWbT = table.Column<float>(type: "real", nullable: true),
                    hxT = table.Column<float>(type: "real", nullable: true),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_runProfileTemplate", x => x.id);
                    table.ForeignKey(
                        name: "FK_runProfileTemplate_statusType_status",
                        column: x => x.status,
                        principalTable: "statusType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "runStepTemplateGroup",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numLoops = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_runStepTemplateGroup", x => x.id);
                    table.ForeignKey(
                        name: "FK_runStepTemplateGroup_statusType_status",
                        column: x => x.status,
                        principalTable: "statusType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "scriptLibrary",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    scriptId = table.Column<int>(type: "int", nullable: false),
                    stepNumber = table.Column<int>(type: "int", nullable: false),
                    phaseLoop = table.Column<int>(type: "int", nullable: true),
                    runStepLibraryWithLoop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scriptLibrary", x => x.id);
                    table.ForeignKey(
                        name: "FK_scriptLibrary_scriptlists_scriptId",
                        column: x => x.scriptId,
                        principalTable: "scriptlists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scriptLibrary_statusType_status",
                        column: x => x.status,
                        principalTable: "statusType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_address_site_sid",
                        column: x => x.sid,
                        principalTable: "site",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    siteID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.id);
                    table.ForeignKey(
                        name: "FK_Contact_site_siteID",
                        column: x => x.siteID,
                        principalTable: "site",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    EqMfgID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EqDes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deviceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    siteID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    configID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    comStatus = table.Column<bool>(type: "bit", nullable: false),
                    h2Production = table.Column<float>(type: "real", nullable: false),
                    powerConsumption = table.Column<float>(type: "real", nullable: false),
                    siteEfficiency = table.Column<float>(type: "real", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nStack = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: true),
                    ver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isRunning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastDataReceivedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dCmd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    mnWbT = table.Column<float>(type: "real", nullable: true),
                    mxWbT = table.Column<float>(type: "real", nullable: true),
                    hxT = table.Column<float>(type: "real", nullable: true)
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
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "stackRunProfileTemplate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    profileID = table.Column<int>(type: "int", nullable: false),
                    loop = table.Column<bool>(type: "bit", nullable: false),
                    command = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stackRunProfileTemplate", x => x.id);
                    table.ForeignKey(
                        name: "FK_stackRunProfileTemplate_runProfileTemplate_profileID",
                        column: x => x.profileID,
                        principalTable: "runProfileTemplate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stackRunProfileTemplate_statusType_status",
                        column: x => x.status,
                        principalTable: "statusType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "deviceData",
                columns: table => new
                {
                    transID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    siteID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    configID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    wL = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    wC = table.Column<float>(type: "real", nullable: true),
                    wT = table.Column<float>(type: "real", nullable: true),
                    hxiT = table.Column<float>(type: "real", nullable: true),
                    hxoT = table.Column<float>(type: "real", nullable: true),
                    wPp = table.Column<float>(type: "real", nullable: true),
                    HYS = table.Column<float>(type: "real", nullable: true),
                    verM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommStatus = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deviceData", x => x.transID);
                    table.ForeignKey(
                        name: "FK_deviceData_device_deviceID",
                        column: x => x.deviceID,
                        principalTable: "device",
                        principalColumn: "EqMfgID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lotusDeviceData",
                columns: table => new
                {
                    transID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    scSn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    siteID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    configID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lotusDeviceData", x => x.transID);
                    table.ForeignKey(
                        name: "FK_lotusDeviceData_device_scSn",
                        column: x => x.scSn,
                        principalTable: "device",
                        principalColumn: "EqMfgID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "runProfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    profileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    dCmd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fan = table.Column<bool>(type: "bit", nullable: true),
                    pump = table.Column<bool>(type: "bit", nullable: true),
                    mnWbT = table.Column<float>(type: "real", nullable: true),
                    mxWbT = table.Column<float>(type: "real", nullable: true),
                    hxT = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_runProfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_runProfile_device_deviceID",
                        column: x => x.deviceID,
                        principalTable: "device",
                        principalColumn: "EqMfgID");
                });

            migrationBuilder.CreateTable(
                name: "stack",
                columns: table => new
                {
                    stackMfgID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    siteID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    stackConfig = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stackPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    meaNum = table.Column<int>(type: "int", nullable: false),
                    meaArea = table.Column<float>(type: "real", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    loop = table.Column<bool>(type: "bit", nullable: true),
                    command = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stack", x => x.stackMfgID);
                    table.ForeignKey(
                        name: "FK_stack_device_deviceID",
                        column: x => x.deviceID,
                        principalTable: "device",
                        principalColumn: "EqMfgID");
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

            migrationBuilder.CreateTable(
                name: "runStepTemplate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stkRunProfileID = table.Column<int>(type: "int", nullable: false),
                    testState = table.Column<int>(type: "int", nullable: false),
                    stepNumber = table.Column<int>(type: "int", nullable: false),
                    sCmd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: false),
                    cI = table.Column<float>(type: "real", nullable: true),
                    cV = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    hP = table.Column<float>(type: "real", nullable: true),
                    wFt = table.Column<float>(type: "real", nullable: true),
                    wTt = table.Column<float>(type: "real", nullable: true),
                    cVt = table.Column<float>(type: "real", nullable: true),
                    cVl = table.Column<float>(type: "real", nullable: true),
                    mnF = table.Column<float>(type: "real", nullable: true),
                    mxF = table.Column<float>(type: "real", nullable: true),
                    rstGID = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    imF = table.Column<float>(type: "real", nullable: true),
                    imA = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_runStepTemplate", x => x.id);
                    table.ForeignKey(
                        name: "FK_runStepTemplate_runStepTemplateGroup_rstGID",
                        column: x => x.rstGID,
                        principalTable: "runStepTemplateGroup",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_runStepTemplate_stackRunProfileTemplate_stkRunProfileID",
                        column: x => x.stkRunProfileID,
                        principalTable: "stackRunProfileTemplate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_runStepTemplate_statusType_status",
                        column: x => x.status,
                        principalTable: "statusType",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_runStepTemplate_testStates_testState",
                        column: x => x.testState,
                        principalTable: "testStates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deviceTemplateAllocation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    templateID = table.Column<int>(type: "int", nullable: false),
                    rptid = table.Column<int>(type: "int", nullable: true),
                    stackID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    stackRunProfileTemplateID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deviceTemplateAllocation", x => x.id);
                    table.ForeignKey(
                        name: "FK_deviceTemplateAllocation_device_deviceID",
                        column: x => x.deviceID,
                        principalTable: "device",
                        principalColumn: "EqMfgID");
                    table.ForeignKey(
                        name: "FK_deviceTemplateAllocation_runProfileTemplate_rptid",
                        column: x => x.rptid,
                        principalTable: "runProfileTemplate",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_deviceTemplateAllocation_stackRunProfileTemplate_stackRunProfileTemplateID",
                        column: x => x.stackRunProfileTemplateID,
                        principalTable: "stackRunProfileTemplate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_deviceTemplateAllocation_stack_stackID",
                        column: x => x.stackID,
                        principalTable: "stack",
                        principalColumn: "stackMfgID");
                });

            migrationBuilder.CreateTable(
                name: "stackPhaseSettingNew",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stackID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    phaseGroup = table.Column<int>(type: "int", nullable: true),
                    phaseLoop = table.Column<int>(type: "int", nullable: true),
                    SequenceListWithLoop = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stackPhaseSettingNew", x => x.id);
                    table.ForeignKey(
                        name: "FK_stackPhaseSettingNew_stack_stackID",
                        column: x => x.stackID,
                        principalTable: "stack",
                        principalColumn: "stackMfgID");
                });

            migrationBuilder.CreateTable(
                name: "stkPhaseSetting",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stackID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    phase = table.Column<int>(type: "int", nullable: false),
                    phaseGroup = table.Column<int>(type: "int", nullable: true),
                    phaseGroupLoop = table.Column<int>(type: "int", nullable: true),
                    rsgid = table.Column<int>(type: "int", nullable: false),
                    loop = table.Column<int>(type: "int", nullable: false),
                    hrs = table.Column<float>(type: "real", nullable: true),
                    totalHours = table.Column<float>(type: "real", nullable: true),
                    mins = table.Column<float>(type: "real", nullable: true),
                    totalMins = table.Column<float>(type: "real", nullable: true),
                    seconds = table.Column<float>(type: "real", nullable: true),
                    hrsminsdisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totalhrsminsdisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    check = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stkPhaseSetting", x => x.id);
                    table.ForeignKey(
                        name: "FK_stkPhaseSetting_runStepTemplateGroup_rsgid",
                        column: x => x.rsgid,
                        principalTable: "runStepTemplateGroup",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stkPhaseSetting_stack_stackID",
                        column: x => x.stackID,
                        principalTable: "stack",
                        principalColumn: "stackMfgID");
                });

            migrationBuilder.CreateTable(
                name: "stkRunProfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    profileID = table.Column<int>(type: "int", nullable: false),
                    stackID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    stackPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    loop = table.Column<bool>(type: "bit", nullable: false),
                    command = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stkRunProfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_stkRunProfile_runProfile_profileID",
                        column: x => x.profileID,
                        principalTable: "runProfile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stkRunProfile_stack_stackID",
                        column: x => x.stackID,
                        principalTable: "stack",
                        principalColumn: "stackMfgID");
                });

            migrationBuilder.CreateTable(
                name: "tsStackData",
                columns: table => new
                {
                    transID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    deviceID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stackMfgID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    wF = table.Column<float>(type: "real", nullable: true),
                    wT = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    hT = table.Column<float>(type: "real", nullable: true),
                    hP = table.Column<float>(type: "real", nullable: true),
                    psI = table.Column<float>(type: "real", nullable: true),
                    psV = table.Column<float>(type: "real", nullable: true),
                    cV1 = table.Column<float>(type: "real", nullable: true),
                    cV2 = table.Column<float>(type: "real", nullable: true),
                    cV3 = table.Column<float>(type: "real", nullable: true),
                    cV4 = table.Column<float>(type: "real", nullable: true),
                    cV5 = table.Column<float>(type: "real", nullable: true),
                    isF = table.Column<float>(type: "real", nullable: true),
                    cV6 = table.Column<float>(type: "real", nullable: true),
                    cR1 = table.Column<float>(type: "real", nullable: true),
                    cR2 = table.Column<float>(type: "real", nullable: true),
                    cR3 = table.Column<float>(type: "real", nullable: true),
                    cR4 = table.Column<float>(type: "real", nullable: true),
                    cR5 = table.Column<float>(type: "real", nullable: true),
                    cX1 = table.Column<float>(type: "real", nullable: true),
                    cX2 = table.Column<float>(type: "real", nullable: true),
                    cX3 = table.Column<float>(type: "real", nullable: true),
                    cX4 = table.Column<float>(type: "real", nullable: true),
                    cX5 = table.Column<float>(type: "real", nullable: true),
                    cR6 = table.Column<float>(type: "real", nullable: true),
                    CD = table.Column<float>(type: "real", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    runHours = table.Column<float>(type: "real", nullable: true),
                    stepNumber = table.Column<int>(type: "int", nullable: true),
                    loopcnt = table.Column<int>(type: "int", nullable: true),
                    cM1 = table.Column<float>(type: "real", nullable: true),
                    cM2 = table.Column<float>(type: "real", nullable: true),
                    cM3 = table.Column<float>(type: "real", nullable: true),
                    cM4 = table.Column<float>(type: "real", nullable: true),
                    cM5 = table.Column<float>(type: "real", nullable: true),
                    seqName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    imF = table.Column<float>(type: "real", nullable: true),
                    imA = table.Column<float>(type: "real", nullable: true),
                    status = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tsStackData", x => x.transID);
                    table.ForeignKey(
                        name: "FK_tsStackData_device_deviceID",
                        column: x => x.deviceID,
                        principalTable: "device",
                        principalColumn: "EqMfgID");
                    table.ForeignKey(
                        name: "FK_tsStackData_stack_stackMfgID",
                        column: x => x.stackMfgID,
                        principalTable: "stack",
                        principalColumn: "stackMfgID");
                });

            migrationBuilder.CreateTable(
                name: "stkStep",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stkRunProfileID = table.Column<int>(type: "int", nullable: false),
                    testState = table.Column<int>(type: "int", nullable: true),
                    stepNumber = table.Column<int>(type: "int", nullable: false),
                    sCmd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: false),
                    cI = table.Column<float>(type: "real", nullable: true),
                    cV = table.Column<float>(type: "real", nullable: true),
                    wP = table.Column<float>(type: "real", nullable: true),
                    hP = table.Column<float>(type: "real", nullable: true),
                    wFt = table.Column<float>(type: "real", nullable: true),
                    wTt = table.Column<float>(type: "real", nullable: true),
                    cVt = table.Column<float>(type: "real", nullable: true),
                    cVl = table.Column<float>(type: "real", nullable: true),
                    mnF = table.Column<float>(type: "real", nullable: true),
                    mxF = table.Column<float>(type: "real", nullable: true),
                    rstGID = table.Column<int>(type: "int", nullable: true),
                    imF = table.Column<float>(type: "real", nullable: true),
                    imA = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stkStep", x => x.id);
                    table.ForeignKey(
                        name: "FK_stkStep_runStepTemplateGroup_rstGID",
                        column: x => x.rstGID,
                        principalTable: "runStepTemplateGroup",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stkStep_stkRunProfile_stkRunProfileID",
                        column: x => x.stkRunProfileID,
                        principalTable: "stkRunProfile",
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
                name: "IX_deviceData_deviceID",
                table: "deviceData",
                column: "deviceID");

            migrationBuilder.CreateIndex(
                name: "IX_deviceTemplateAllocation_deviceID",
                table: "deviceTemplateAllocation",
                column: "deviceID");

            migrationBuilder.CreateIndex(
                name: "IX_deviceTemplateAllocation_rptid",
                table: "deviceTemplateAllocation",
                column: "rptid");

            migrationBuilder.CreateIndex(
                name: "IX_deviceTemplateAllocation_stackID",
                table: "deviceTemplateAllocation",
                column: "stackID");

            migrationBuilder.CreateIndex(
                name: "IX_deviceTemplateAllocation_stackRunProfileTemplateID",
                table: "deviceTemplateAllocation",
                column: "stackRunProfileTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_lotusDeviceData_scSn",
                table: "lotusDeviceData",
                column: "scSn");

            migrationBuilder.CreateIndex(
                name: "IX_runProfile_deviceID",
                table: "runProfile",
                column: "deviceID");

            migrationBuilder.CreateIndex(
                name: "IX_runProfileTemplate_status",
                table: "runProfileTemplate",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_runstepLibrary_seqMasterId",
                table: "runstepLibrary",
                column: "seqMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_runStepTemplate_rstGID",
                table: "runStepTemplate",
                column: "rstGID");

            migrationBuilder.CreateIndex(
                name: "IX_runStepTemplate_status",
                table: "runStepTemplate",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_runStepTemplate_stkRunProfileID",
                table: "runStepTemplate",
                column: "stkRunProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_runStepTemplate_testState",
                table: "runStepTemplate",
                column: "testState");

            migrationBuilder.CreateIndex(
                name: "IX_runStepTemplateGroup_status",
                table: "runStepTemplateGroup",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_scriptLibrary_scriptId",
                table: "scriptLibrary",
                column: "scriptId");

            migrationBuilder.CreateIndex(
                name: "IX_scriptLibrary_status",
                table: "scriptLibrary",
                column: "status");

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
                name: "IX_stack_status",
                table: "stack",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_stackPhaseSettingNew_stackID",
                table: "stackPhaseSettingNew",
                column: "stackID");

            migrationBuilder.CreateIndex(
                name: "IX_stackRunProfileTemplate_profileID",
                table: "stackRunProfileTemplate",
                column: "profileID");

            migrationBuilder.CreateIndex(
                name: "IX_stackRunProfileTemplate_status",
                table: "stackRunProfileTemplate",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_stackSyncData_scriptID",
                table: "stackSyncData",
                column: "scriptID");

            migrationBuilder.CreateIndex(
                name: "IX_stkPhaseSetting_rsgid",
                table: "stkPhaseSetting",
                column: "rsgid");

            migrationBuilder.CreateIndex(
                name: "IX_stkPhaseSetting_stackID",
                table: "stkPhaseSetting",
                column: "stackID");

            migrationBuilder.CreateIndex(
                name: "IX_stkRunProfile_profileID",
                table: "stkRunProfile",
                column: "profileID");

            migrationBuilder.CreateIndex(
                name: "IX_stkRunProfile_stackID",
                table: "stkRunProfile",
                column: "stackID");

            migrationBuilder.CreateIndex(
                name: "IX_stkStep_rstGID",
                table: "stkStep",
                column: "rstGID");

            migrationBuilder.CreateIndex(
                name: "IX_stkStep_stkRunProfileID",
                table: "stkStep",
                column: "stkRunProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_tsStackData_deviceID",
                table: "tsStackData",
                column: "deviceID");

            migrationBuilder.CreateIndex(
                name: "IX_tsStackData_stackMfgID",
                table: "tsStackData",
                column: "stackMfgID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "colorConfig");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "deviceData");

            migrationBuilder.DropTable(
                name: "deviceTemplateAllocation");

            migrationBuilder.DropTable(
                name: "feedback");

            migrationBuilder.DropTable(
                name: "lotusDeviceData");

            migrationBuilder.DropTable(
                name: "lotusTempdata");

            migrationBuilder.DropTable(
                name: "mmp");

            migrationBuilder.DropTable(
                name: "mtsDeviceData");

            migrationBuilder.DropTable(
                name: "mtsDeviceDataNew");

            migrationBuilder.DropTable(
                name: "mtsDeviceDataNew2");

            migrationBuilder.DropTable(
                name: "mtsStackData");

            migrationBuilder.DropTable(
                name: "mtsStackDataNew");

            migrationBuilder.DropTable(
                name: "mtsStackDataNew2");

            migrationBuilder.DropTable(
                name: "runstepLibrary");

            migrationBuilder.DropTable(
                name: "runStepTemplate");

            migrationBuilder.DropTable(
                name: "sconfig");

            migrationBuilder.DropTable(
                name: "scriptLibrary");

            migrationBuilder.DropTable(
                name: "segment");

            migrationBuilder.DropTable(
                name: "stackPhaseSettingNew");

            migrationBuilder.DropTable(
                name: "StacksThatRan");

            migrationBuilder.DropTable(
                name: "stackSyncData");

            migrationBuilder.DropTable(
                name: "stackTestRunHours");

            migrationBuilder.DropTable(
                name: "stkPhaseSetting");

            migrationBuilder.DropTable(
                name: "stkStep");

            migrationBuilder.DropTable(
                name: "sysMaster");

            migrationBuilder.DropTable(
                name: "testProfileConfig");

            migrationBuilder.DropTable(
                name: "thresholdconfigs");

            migrationBuilder.DropTable(
                name: "tsStackData");

            migrationBuilder.DropTable(
                name: "userLogin");

            migrationBuilder.DropTable(
                name: "sequencyLibrary");

            migrationBuilder.DropTable(
                name: "stackRunProfileTemplate");

            migrationBuilder.DropTable(
                name: "testStates");

            migrationBuilder.DropTable(
                name: "scriptlists");

            migrationBuilder.DropTable(
                name: "runStepTemplateGroup");

            migrationBuilder.DropTable(
                name: "stkRunProfile");

            migrationBuilder.DropTable(
                name: "runProfileTemplate");

            migrationBuilder.DropTable(
                name: "runProfile");

            migrationBuilder.DropTable(
                name: "stack");

            migrationBuilder.DropTable(
                name: "device");

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
