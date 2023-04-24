using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentParser.Client
{
    public class Parser
    {
        private static Dictionary<int, string> _regions = new Dictionary<int, string>()
        {
            { 1   ,"г.Ташкент" },
            { 2   ,"Андижанский вилоят" },
            { 3   ,"Бухарский вилоят" },
            { 4   ,"Джизакский вилоят" },
            { 5   ,"Кашкадарьинская область" },
            { 6   ,"Навоийский вилоят" },
            { 7   ,"Наманганский вилоят" },
            { 8   ,"Респ. Каракалпакистан" },
            { 9   ,"Самаркандский вилоят" },
            { 10  ,"Сурхандарьинский вилоят" },
            { 11  ,"Самарканская область" },
            { 12  ,"Ташкентская Область" },
            { 13  ,"Ферганский вилоят" },
            { 14  ,"Хорезамская область" }
        };

        private static List<Tuple<int, string, int>> _areas = new List<Tuple<int, string, int>>()
        {
            new Tuple<int, string, int>(1, "Юнусабадский р-н", 1),
            new Tuple<int, string, int>(2, "Бектемирский р-н", 1),
            new Tuple<int, string, int>(3, "Шайхантахурский р-н", 1),
            new Tuple<int, string, int>(4, "Мирзо Улугбекский р-н", 1),
            new Tuple<int, string, int>(5, "Яшнободский р-н", 1),
            new Tuple<int, string, int>(6, "Сергелийский р-н", 1),
            new Tuple<int, string, int>(7, "Чиланзарский р-н", 1),
            new Tuple<int, string, int>(8, "Олмазорский р-н", 1),
            new Tuple<int, string, int>(9, "Учтепинский р-н", 1),
            new Tuple<int, string, int>(10, "Мирабадский р-н", 1),
            new Tuple<int, string, int>(11, "Яккасарайский р-н", 1),
            new Tuple<int, string, int>(12, "Янгихаётский р-н", 1),
            new Tuple<int, string, int>(13, "Кургантепа", 2),
            new Tuple<int, string, int>(14, "Ходжаобод", 2),
            new Tuple<int, string, int>(15, "Шахрихан", 2),
            new Tuple<int, string, int>(16, "Шахрихан (г.)", 2),
            new Tuple<int, string, int>(17, "Ханабад (г.)", 2),
            new Tuple<int, string, int>(18, "Чинобод (г.)", 2),
            new Tuple<int, string, int>(19, "Пайтуг (г.)", 2),
            new Tuple<int, string, int>(20, "Карасув (г.)", 2),
            new Tuple<int, string, int>(21, "Ахунбабаев", 2),
            new Tuple<int, string, int>(22, "Асака", 2),
            new Tuple<int, string, int>(23, "Асака (г.)", 2),
            new Tuple<int, string, int>(24, "Баликчи", 2),
            new Tuple<int, string, int>(25, "Андижан (г.)", 2),
            new Tuple<int, string, int>(26, "Чинобод", 2),
            new Tuple<int, string, int>(27, "Андижан г.(Новый)", 2),
            new Tuple<int, string, int>(28, "Андижан", 2),
            new Tuple<int, string, int>(29, "Булакбаши", 2),
            new Tuple<int, string, int>(30, "Олтинкул", 2),
            new Tuple<int, string, int>(31, "Пахтаабад", 2),
            new Tuple<int, string, int>(32, "Улугнор", 2),
            new Tuple<int, string, int>(33, "Мархамат", 2),
            new Tuple<int, string, int>(34, "Боз", 2),
            new Tuple<int, string, int>(35, "Жалолкудук", 2),
            new Tuple<int, string, int>(36, "Избоскган", 2),
            new Tuple<int, string, int>(37, "Гиждуван", 3),
            new Tuple<int, string, int>(38, "Гиждуван (г.)", 3),
            new Tuple<int, string, int>(39, "Каракуль", 3),
            new Tuple<int, string, int>(40, "Жондор", 3),
            new Tuple<int, string, int>(41, "Когон", 3),
            new Tuple<int, string, int>(42, "Когон (г.)", 3),
            new Tuple<int, string, int>(43, "Бухара", 3),
            new Tuple<int, string, int>(44, "Бухара  (г.)", 3),
            new Tuple<int, string, int>(45, "Вабкент", 3),
            new Tuple<int, string, int>(46, "Олот", 3),
            new Tuple<int, string, int>(47, "Ромитан", 3),
            new Tuple<int, string, int>(48, "Пешку", 3),
            new Tuple<int, string, int>(49, "Караулбазар", 3),
            new Tuple<int, string, int>(50, "Шафиркан", 3),
            new Tuple<int, string, int>(51, "Пахтакор", 4),
            new Tuple<int, string, int>(52, "Фариш", 4),
            new Tuple<int, string, int>(53, "Зомин", 4),
            new Tuple<int, string, int>(54, "Мирзачуль", 4),
            new Tuple<int, string, int>(55, "Янгиабад", 4),
            new Tuple<int, string, int>(56, "Даштобод (г.)", 4),
            new Tuple<int, string, int>(57, "Арнасай", 4),
            new Tuple<int, string, int>(58, "Галлаарал", 4),
            new Tuple<int, string, int>(59, "Джизак (г.)", 4),
            new Tuple<int, string, int>(60, "Зафарабад", 4),
            new Tuple<int, string, int>(61, "Джизак", 4),
            new Tuple<int, string, int>(62, "Дустлик", 4),
            new Tuple<int, string, int>(63, "Бахмал", 4),
            new Tuple<int, string, int>(64, "Зарбдар", 4),
            new Tuple<int, string, int>(65, "Шахрисабз", 5),
            new Tuple<int, string, int>(66, "Чирокчи", 5),
            new Tuple<int, string, int>(67, "Миришкор", 5),
            new Tuple<int, string, int>(68, "Бешкент", 5),
            new Tuple<int, string, int>(69, "Яккабог", 5),
            new Tuple<int, string, int>(70, "Карши", 5),
            new Tuple<int, string, int>(71, "Касби", 5),
            new Tuple<int, string, int>(72, "Бахористон", 5),
            new Tuple<int, string, int>(73, "Гузор", 5),
            new Tuple<int, string, int>(74, "Дехконобод", 5),
            new Tuple<int, string, int>(75, "Камаши", 5),
            new Tuple<int, string, int>(76, "Мубарек", 5),
            new Tuple<int, string, int>(77, "Косон", 5),
            new Tuple<int, string, int>(78, "Нишон", 5),
            new Tuple<int, string, int>(79, "Усмон Юсупов", 5),
            new Tuple<int, string, int>(80, "Китоб", 5),
            new Tuple<int, string, int>(81, "Навои", 6),
            new Tuple<int, string, int>(82, "Янгиробод", 6),
            new Tuple<int, string, int>(83, "Ширин (г.)", 6),
            new Tuple<int, string, int>(84, "Навои (г.)", 6),
            new Tuple<int, string, int>(85, "Кармана", 6),
            new Tuple<int, string, int>(86, "Учкудук", 6),
            new Tuple<int, string, int>(87, "Учкудук (г.)", 6),
            new Tuple<int, string, int>(88, "Зарафшан (г.)", 6),
            new Tuple<int, string, int>(89, "Хатырчи", 6),
            new Tuple<int, string, int>(90, "Кызылтепа", 6),
            new Tuple<int, string, int>(91, "Канимех", 6),
            new Tuple<int, string, int>(92, "Навбахор", 6),
            new Tuple<int, string, int>(93, "Томди", 6),
            new Tuple<int, string, int>(94, "Нурата", 6),
            new Tuple<int, string, int>(95, "Хаккулобод", 7),
            new Tuple<int, string, int>(96, "Учкурган (г.)", 7),
            new Tuple<int, string, int>(97, "Чорток", 7),
            new Tuple<int, string, int>(98, "Туракурган", 7),
            new Tuple<int, string, int>(99, "Учкурган", 7),
            new Tuple<int, string, int>(100, "Чуст (г.)", 7),
            new Tuple<int, string, int>(101, "Янгикурган", 7),
            new Tuple<int, string, int>(102, "Чорток (г.)", 7),
            new Tuple<int, string, int>(103, "Чуст", 7),
            new Tuple<int, string, int>(104, "Уйчи", 7),
            new Tuple<int, string, int>(105, "Касансай (г.)", 7),
            new Tuple<int, string, int>(106, "Минг Булок", 7),
            new Tuple<int, string, int>(107, "Давлатобод", 7),
            new Tuple<int, string, int>(108, "Касансай", 7),
            new Tuple<int, string, int>(109, "Норин", 7),
            new Tuple<int, string, int>(110, "Поп", 7),
            new Tuple<int, string, int>(111, "Наманган", 7),
            new Tuple<int, string, int>(112, "Наманган (г.)", 7),
            new Tuple<int, string, int>(113, "Шымбай", 8),
            new Tuple<int, string, int>(114, "Акмангит (г.)", 8),
            new Tuple<int, string, int>(115, "Бустон (г.)", 8),
            new Tuple<int, string, int>(116, "Мангит", 8),
            new Tuple<int, string, int>(117, "Элликкала", 8),
            new Tuple<int, string, int>(118, "Канлыкуль", 8),
            new Tuple<int, string, int>(119, "Караузяк", 8),
            new Tuple<int, string, int>(120, "Турткуль (г.)", 8),
            new Tuple<int, string, int>(121, "Тахиаташ (г.)", 8),
            new Tuple<int, string, int>(122, "Беруний (г.)", 8),
            new Tuple<int, string, int>(123, "Кунград (г.)", 8),
            new Tuple<int, string, int>(124, "Чимбай (г.)", 8),
            new Tuple<int, string, int>(125, "Ходжейли (г.)", 8),
            new Tuple<int, string, int>(126, "Кегейли", 8),
            new Tuple<int, string, int>(127, "Муйнак", 8),
            new Tuple<int, string, int>(128, "Нукус", 8),
            new Tuple<int, string, int>(129, "Амударьё", 8),
            new Tuple<int, string, int>(130, "Беруний", 8),
            new Tuple<int, string, int>(131, "Бузотов", 8),
            new Tuple<int, string, int>(132, "Тахтакупыр", 8),
            new Tuple<int, string, int>(133, "Кунград", 8),
            new Tuple<int, string, int>(134, "Чимбай", 8),
            new Tuple<int, string, int>(135, "Шуманай", 8),
            new Tuple<int, string, int>(136, "Турткуль", 8),
            new Tuple<int, string, int>(137, "Ходжейли", 8),
            new Tuple<int, string, int>(138, "Нукус (г.)", 8),
            new Tuple<int, string, int>(139, "Темирйул", 9),
            new Tuple<int, string, int>(140, "Челак (г.)", 9),
            new Tuple<int, string, int>(141, "г. Лоиш", 9),
            new Tuple<int, string, int>(142, "Булунгур", 9),
            new Tuple<int, string, int>(143, "Каттакурган", 9),
            new Tuple<int, string, int>(144, "Иштыхан", 9),
            new Tuple<int, string, int>(145, "Джамбай", 9),
            new Tuple<int, string, int>(146, "Багишамальский", 9),
            new Tuple<int, string, int>(147, "Дустлик", 9),
            new Tuple<int, string, int>(148, "Корадарё", 9),
            new Tuple<int, string, int>(149, "Сиабский", 9),
            new Tuple<int, string, int>(150, "г. Джума", 9),
            new Tuple<int, string, int>(151, "Гузалкент", 9),
            new Tuple<int, string, int>(152, "Дехканабад", 9),
            new Tuple<int, string, int>(153, "Каттакурган (г.)", 9),
            new Tuple<int, string, int>(154, "Ургут", 9),
            new Tuple<int, string, int>(155, "Тайляк", 9),
            new Tuple<int, string, int>(156, "Самарканд (г.)", 9),
            new Tuple<int, string, int>(157, "Ургут (г.)", 9),
            new Tuple<int, string, int>(158, "Зияддин", 9),
            new Tuple<int, string, int>(159, "Акташ", 9),
            new Tuple<int, string, int>(160, "Кушработ (г.)", 9),
            new Tuple<int, string, int>(161, "Окдарё", 9),
            new Tuple<int, string, int>(162, "Пайарык", 9),
            new Tuple<int, string, int>(163, "Нарпай", 9),
            new Tuple<int, string, int>(164, "Нурабад", 9),
            new Tuple<int, string, int>(165, "Самарканд", 9),
            new Tuple<int, string, int>(166, "Пахтачи", 9),
            new Tuple<int, string, int>(167, "Пастдаргом", 9),
            new Tuple<int, string, int>(168, "Шаргун", 10),
            new Tuple<int, string, int>(169, "Термез", 10),
            new Tuple<int, string, int>(170, "Сариасиё", 10),
            new Tuple<int, string, int>(171, "Олтинсой", 10),
            new Tuple<int, string, int>(172, "Термез (г.)", 10),
            new Tuple<int, string, int>(173, "Шурчи", 10),
            new Tuple<int, string, int>(174, "Шерабад", 10),
            new Tuple<int, string, int>(175, "Узун", 10),
            new Tuple<int, string, int>(176, "Музрабат", 10),
            new Tuple<int, string, int>(177, "Бандихан", 10),
            new Tuple<int, string, int>(178, "Байсун", 10),
            new Tuple<int, string, int>(179, "Ангор", 10),
            new Tuple<int, string, int>(180, "Денау", 10),
            new Tuple<int, string, int>(181, "Кумкурган", 10),
            new Tuple<int, string, int>(182, "Кизирик", 10),
            new Tuple<int, string, int>(183, "Джаркурган", 10),
            new Tuple<int, string, int>(184, "Денау (г.)", 10),
            new Tuple<int, string, int>(185, "Гулистан", 11),
            new Tuple<int, string, int>(186, "Гулистан (г.)", 11),
            new Tuple<int, string, int>(187, "Мехнатабад", 11),
            new Tuple<int, string, int>(188, "Баяут", 11),
            new Tuple<int, string, int>(189, "Пахтаобод", 11),
            new Tuple<int, string, int>(190, "Сардоба", 11),
            new Tuple<int, string, int>(191, "Акалтын", 11),
            new Tuple<int, string, int>(192, "Мирзаобод", 11),
            new Tuple<int, string, int>(193, "Шараф Рашидов", 11),
            new Tuple<int, string, int>(194, "Ширин (г.)", 11),
            new Tuple<int, string, int>(195, "Янгиер (г.)", 11),
            new Tuple<int, string, int>(196, "Ховос", 11),
            new Tuple<int, string, int>(197, "Сайхунабад", 11),
            new Tuple<int, string, int>(198, "Сырдарья", 11),
            new Tuple<int, string, int>(199, "Сырдарья (г.)", 11),
            new Tuple<int, string, int>(200, "Бахт (г.)", 11),
            new Tuple<int, string, int>(201, "Алмалык", 12),
            new Tuple<int, string, int>(202, "Янгийул", 12),
            new Tuple<int, string, int>(203, "Газалкент", 12),
            new Tuple<int, string, int>(204, "Зафар (г)", 12),
            new Tuple<int, string, int>(205, "Чирчик (г.)", 12),
            new Tuple<int, string, int>(206, "Ангрен", 12),
            new Tuple<int, string, int>(207, "Янгибазар", 12),
            new Tuple<int, string, int>(208, "Юкори Чирчик", 12),
            new Tuple<int, string, int>(209, "Янгиюль(г)", 12),
            new Tuple<int, string, int>(210, "Нуравшан", 12),
            new Tuple<int, string, int>(211, "Келес (г.)", 12),
            new Tuple<int, string, int>(212, "Хасанбой", 12),
            new Tuple<int, string, int>(213, "Чиноз", 12),
            new Tuple<int, string, int>(214, "Зангиота", 12),
            new Tuple<int, string, int>(215, "Кибрай", 12),
            new Tuple<int, string, int>(216, "Куйи Чирчик", 12),
            new Tuple<int, string, int>(217, "Бекабад", 12),
            new Tuple<int, string, int>(218, "Бука", 12),
            new Tuple<int, string, int>(219, "Бустонлик", 12),
            new Tuple<int, string, int>(220, "Пискент", 12),
            new Tuple<int, string, int>(221, "Ташкент", 12),
            new Tuple<int, string, int>(222, "Урта Чирчик", 12),
            new Tuple<int, string, int>(223, "Оккурган", 12),
            new Tuple<int, string, int>(224, "Охангарон", 12),
            new Tuple<int, string, int>(225, "Паркент", 12),
            new Tuple<int, string, int>(226, "Узбекистон", 13),
            new Tuple<int, string, int>(227, "Фергана", 13),
            new Tuple<int, string, int>(228, "Ташлак", 13),
            new Tuple<int, string, int>(229, "Учкуприк", 13),
            new Tuple<int, string, int>(230, "Сох", 13),
            new Tuple<int, string, int>(231, "Кува", 13),
            new Tuple<int, string, int>(232, "Дангара", 13),
            new Tuple<int, string, int>(233, "Бувайда", 13),
            new Tuple<int, string, int>(234, "Риштан", 13),
            new Tuple<int, string, int>(235, "Навбахор (г.)", 13),
            new Tuple<int, string, int>(236, "Кургантепа", 13),
            new Tuple<int, string, int>(237, "Кува (г.)", 13),
            new Tuple<int, string, int>(238, "Кувасай (г.)", 13),
            new Tuple<int, string, int>(239, "Алтыарик", 13),
            new Tuple<int, string, int>(240, "Маргилан (г.)", 13),
            new Tuple<int, string, int>(241, "Коканд (г.)", 13),
            new Tuple<int, string, int>(242, "Куштепа", 13),
            new Tuple<int, string, int>(243, "Фергана (г.)", 13),
            new Tuple<int, string, int>(244, "Яйпан", 13),
            new Tuple<int, string, int>(245, "Бешарик", 13),
            new Tuple<int, string, int>(246, "Язъяван", 13),
            new Tuple<int, string, int>(247, "Фуркат", 13),
            new Tuple<int, string, int>(248, "Багдад", 13),
            new Tuple<int, string, int>(249, "Киргули", 13),
            new Tuple<int, string, int>(250, "Водил", 13),
            new Tuple<int, string, int>(251, "Ахунбабаев", 13),
            new Tuple<int, string, int>(252, "Янгибазар", 14),
            new Tuple<int, string, int>(253, "Янгиарык", 14),
            new Tuple<int, string, int>(254, "Шават", 14),
            new Tuple<int, string, int>(255, "Питнак  (г.)", 14),
            new Tuple<int, string, int>(256, "Хазаразп", 14),
            new Tuple<int, string, int>(257, "Кушкупыр", 14),
            new Tuple<int, string, int>(258, "Ханка", 14),
            new Tuple<int, string, int>(259, "Ургенч", 14),
            new Tuple<int, string, int>(260, "Гурлан", 14),
            new Tuple<int, string, int>(261, "Багат", 14),
            new Tuple<int, string, int>(262, "Хива (г.)", 14),
            new Tuple<int, string, int>(263, "Хива", 14),
            new Tuple<int, string, int>(264, "Ургенч (г.)", 14)
        };

        public static string ParseAndGetSql(string fileName)
        {

            Dictionary<string, int> sql = new Dictionary<string, int>()
            {
                { "Наманган", 7 },
                { "Фергана", 13 },
                { "Тошкент ш.-", 1 },
                { "Республиканс г Ташкент", 1 },
                { "Тош вил", 12 },
                { "Сирдарье вил", 11 },
                { "Кораколпок", 8 },
                { "Хоразм", 14 },
                { "Бухоро", 3 },
                { "Навоий", 6 },
                { "Кашкадаре", 5 },
                { "Сурхандарья", 10 },
                { "Жиззах", 4 },
                { "Самарканд", 9 },
                { "Андижон", 2 },
            };

            FileInfo fileInfo = new FileInfo(fileName);

            StringBuilder stringBuilder = new StringBuilder();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Dimension == null)
                        continue;

                    int columnCount = worksheet.Dimension.End.Column;
                    int rowsCount = worksheet.Dimension.End.Row;

                    for (int i = 1; i <= rowsCount; i++)
                    {
                        string orgName = worksheet.Cells[i, 2].Value?.ToString()?.Trim();
                        string orgAddress = worksheet.Cells[i, 3].Value?.ToString()?.Trim();
                        string phone = columnCount >= 3  ? worksheet.Cells[i, 4].Value?.ToString()?.Trim()?.Replace("-", ""): "";

                        int regionId = sql.ContainsKey(worksheet.Name.Trim()) ? sql[worksheet.Name.Trim()] : 0;

                        if (string.IsNullOrEmpty(orgName))
                            continue;

                        stringBuilder.Append($"(2, N'{orgName}', N'{orgAddress}', '{phone}', {regionId}),\n");
                    }
                }
            }


            return stringBuilder.ToString();
        }

        public static string ParseAndGetSql1(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            StringBuilder stringBuilder = new StringBuilder();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Dimension == null)
                        continue;

                    int columnCount = worksheet.Dimension.End.Column;
                    int rowsCount = worksheet.Dimension.End.Row;

                    for (int i = 1; i <= rowsCount; i++)
                    {
                        if (i % 1000 == 1)
                        {
                            stringBuilder.AppendLine("INSERT INTO DICT_ORGANIZATIONS (Name, Inn, RegionId, AreaId, TypeId)");
                            stringBuilder.AppendLine("VALUES");
                        }

                        int type = 1;
                        string name = worksheet.Cells[i, 2].Value?.ToString().Replace("'", "''").Replace("\"", "");

                        if (string.IsNullOrEmpty(name?.Trim()))
                            continue;

                        string inn = worksheet.Cells[i, 3].Value?.ToString();

                        string region = worksheet.Cells[i, 4].Value?.ToString();

                        var rgb = worksheet.Cells[i, 1, i, 4].Style.Fill;

                        if (rgb.BackgroundColor.Rgb == "FF92D050")
                        {
                            type = 3;
                        }
                        int? regionId = null, areaId = null;

                        if (!string.IsNullOrEmpty(region))
                        {
                            if (_regions.ContainsValue(region))
                            {
                                regionId = _regions.FirstOrDefault(f => f.Value == region).Key;
                            }
                            else if (_areas.Exists(e => e.Item2.ToLower().Contains(region.ToLower())))
                            {
                                var area = _areas.FirstOrDefault(e => e.Item2.ToLower().Contains(region.ToLower()));

                                regionId = area.Item3;
                                areaId = area.Item1;
                            }
                        }

                        stringBuilder.AppendLine($"('{name}', '{inn}', {(regionId == null ? "NULL" : regionId.ToString())}, {(areaId == null ? "NULL" : areaId.ToString())}, {type}),");
                    }
                }
            }


            return stringBuilder.ToString();
        }
    }
}
