using System;
using System.Collections.Generic;
using LoxQuest3D.Core;
using LoxQuest3D.World;

namespace LoxQuest3D.Encounters.Procedural
{
    public static class EncounterTemplates
    {
        public static IEnumerable<EncounterDefinition> BuildCommon()
        {
            yield return ScamVendor();
            yield return InfoGypsyCourse();
            yield return GypsyFortune();
            yield return StreetCharity();
            yield return RandomFine();
            yield return SubscriptionTrap();
            yield return BrokenPurchase();
            yield return SurpriseCommission();
            yield return NeighborDrill();
            yield return ParcelScam();
            yield return KioskBait();
            yield return BarTalk();
            yield return PharmacyStickerShock();
            yield return PawnshopDeal();
            yield return DkPoster();
            yield return RailCrossingDelay();
            yield return BusStationSnack();
            yield return MineGateShift();
        }

        private static EncounterDefinition ScamVendor()
        {
            return new EncounterDefinition
            {
                id = "proc_scam_vendor",
                title = "Продавец счастья",
                body = "К тебе подлетает продавец «пылесоса за 200к» и уже держит договор возле твоей руки.",
                allowedLocations = new List<int> { (int)LocationId.Yard, (int)LocationId.Market, (int)LocationId.Store },
                weight = 4,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Взять «в рассрочку»", moneyDelta = -6000, stressDelta = 6, resultText = "Рассрочка оказалась «навсегда». Ты оплатил первый взнос и чувство стыда." },
                    new EncounterChoice { label = "Сказать «у меня уже есть»", moneyDelta = -200, stressDelta = 2, resultText = "Он предложил расширенную гарантию. Ты сам не понял как заплатил." },
                    new EncounterChoice { label = "Уйти", moneyDelta = 0, stressDelta = 1, resultText = "Ты ушёл. Он всё равно успел вручить листовку, которая списала 1% заряда телефона." }
                }
            };
        }

        private static EncounterDefinition InfoGypsyCourse()
        {
            return new EncounterDefinition
            {
                id = "proc_infogypsy_course",
                title = "Курс успешного успеха",
                body = "Инфо-коуч в худи говорит: «У тебя потенциал на миллион. Осталось купить доступ».",
                allowedLocations = new List<int> { (int)LocationId.Park, (int)LocationId.Cafe, (int)LocationId.BusStop },
                weight = 4,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Купить доступ", moneyDelta = -1500, stressDelta = 4, resultText = "Тебе прислали PDF на 2 страницы: «верь в себя». Платёж невозвратный." },
                    new EncounterChoice { label = "Попросить пробный урок", moneyDelta = -300, stressDelta = 2, resultText = "Пробный урок оказался платным. И ещё подпиской на 12 месяцев." },
                    new EncounterChoice { label = "Отшутиться и уйти", moneyDelta = -100, stressDelta = 1, resultText = "Он улыбнулся: «юмор — признак бедности». С тебя списали «донат»." }
                }
            };
        }

        private static EncounterDefinition GypsyFortune()
        {
            return new EncounterDefinition
            {
                id = "proc_gypsy_fortune",
                title = "Судьба на ладони",
                body = "Цыганка берёт твою руку: «Ой, беда-беда, снимаю за малую монету».",
                allowedLocations = new List<int> { (int)LocationId.Yard, (int)LocationId.BusStop, (int)LocationId.Market },
                weight = 3,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Дать 200₽", moneyDelta = -200, stressDelta = 2, resultText = "Беда снялась. Но появилась новая: «надо закрепить результат»." },
                    new EncounterChoice { label = "Сказать, что нет налички", moneyDelta = -150, stressDelta = 2, resultText = "«Переводом можно». Ты сам назвал сумму и заплакал внутри." },
                    new EncounterChoice { label = "Убежать", moneyDelta = 0, stressDelta = 3, resultText = "Ты убежал, но теперь ты плохой человек в её истории. Это тревожит." }
                }
            };
        }

        private static EncounterDefinition StreetCharity()
        {
            return new EncounterDefinition
            {
                id = "proc_street_charity",
                title = "Благотворительность",
                body = "Тебе суют коробку: «На котиков. Ну хотя бы чуть-чуть».",
                allowedLocations = new List<int> { (int)LocationId.Store, (int)LocationId.Park, (int)LocationId.Market },
                weight = 3,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Пожертвовать 50₽", moneyDelta = -50, stressDelta = -1, resultText = "Ты сделал доброе дело. Коробка оказалась для сбора на новую коробку." },
                    new EncounterChoice { label = "Пожертвовать 300₽", moneyDelta = -300, stressDelta = 1, resultText = "Тебя поблагодарили и добавили в рассылку. Теперь котики каждый день." },
                    new EncounterChoice { label = "Отказаться", moneyDelta = -30, stressDelta = 2, resultText = "На тебя посмотрели так, что ты почувствовал вину. Вина — платная услуга." }
                }
            };
        }

        private static EncounterDefinition RandomFine()
        {
            return new EncounterDefinition
            {
                id = "proc_random_fine",
                title = "Штраф из ниоткуда",
                body = "Приходит уведомление: «Штраф за неправильное существование».",
                allowedLocations = new List<int> { (int)LocationId.Apartment, (int)LocationId.Office, (int)LocationId.BusStop },
                weight = 2,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Оплатить сразу", moneyDelta = -700, stressDelta = 2, resultText = "Ты оплатил. Комиссия комиссии тоже оплачена." },
                    new EncounterChoice { label = "Оспорить", moneyDelta = -400, stressDelta = 6, resultText = "Ты оспорил. В итоге штраф удвоили, но тебе сделали скидку на бумагу." },
                    new EncounterChoice { label = "Игнорировать", moneyDelta = -900, stressDelta = 3, resultText = "Игнорирование — тоже услуга. Через день пришла пеня." }
                }
            };
        }

        private static EncounterDefinition SubscriptionTrap()
        {
            return new EncounterDefinition
            {
                id = "proc_subscription_trap",
                title = "Подписка на воздух",
                body = "Твоё приложение «Погода» стало платным. У тебя нет выбора, но он есть.",
                allowedLocations = new List<int> { (int)LocationId.Apartment, (int)LocationId.Office, (int)LocationId.Cafe },
                weight = 2,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Оформить подписку", moneyDelta = -199, stressDelta = 1, resultText = "Теперь ты знаешь, что дождь — это дорого." },
                    new EncounterChoice { label = "Удалить приложение", moneyDelta = -99, stressDelta = 2, resultText = "Удаление платное. Спасибо за лояльность." },
                    new EncounterChoice { label = "Нажать «позже»", moneyDelta = -149, stressDelta = 2, resultText = "Ты нажал «позже». Позже наступило сразу." }
                }
            };
        }

        private static EncounterDefinition BrokenPurchase()
        {
            return new EncounterDefinition
            {
                id = "proc_broken_purchase",
                title = "Покупка дня",
                body = "Ты покупаешь вещь. Дома она превращается в другой предмет.",
                allowedLocations = new List<int> { (int)LocationId.Store, (int)LocationId.Market, (int)LocationId.ServiceCenter },
                weight = 3,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Вернуть по чеку", moneyDelta = -350, stressDelta = 4, resultText = "Чек распечатан на термобумаге и уже белый. Возврат — за доплату." },
                    new EncounterChoice { label = "Ремонтировать", moneyDelta = -500, stressDelta = 3, resultText = "Ремонт дороже покупки. Но теперь у тебя есть опыт." },
                    new EncounterChoice { label = "Смириться", moneyDelta = 0, stressDelta = 2, resultText = "Ты смирился. Смирение усиливает последующие покупки." }
                }
            };
        }

        private static EncounterDefinition SurpriseCommission()
        {
            return new EncounterDefinition
            {
                id = "proc_surprise_commission",
                title = "Комиссия за комиссию",
                body = "Терминал пишет: «Комиссия 9%». Ниже: «Комиссия за комиссию 4%».",
                allowedLocations = new List<int> { (int)LocationId.Store, (int)LocationId.Cafe, (int)LocationId.ServiceCenter },
                weight = 3,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Оплатить картой", moneyDelta = -350, stressDelta = 2, resultText = "Ты оплатил. Терминал поблагодарил тебя и твоих потомков." },
                    new EncounterChoice { label = "Оплатить наличкой", moneyDelta = -250, stressDelta = 2, resultText = "Сдачи нет. «Можем на телефон». Комиссия тоже на телефон." },
                    new EncounterChoice { label = "Отказаться от покупки", moneyDelta = -120, stressDelta = 3, resultText = "Отказ платный. Это город возможностей." }
                }
            };
        }

        private static EncounterDefinition NeighborDrill()
        {
            return new EncounterDefinition
            {
                id = "proc_neighbor_drill",
                title = "Перфоратор судьбы",
                body = "Сосед сверлит стену. Кажется, он сверлит твою психику.",
                allowedLocations = new List<int> { (int)LocationId.Apartment },
                weight = 5,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Постучать", moneyDelta = -100, stressDelta = 4, resultText = "Ты постучал. Он сверлит громче. С тебя «за инициативу»." },
                    new EncounterChoice { label = "Включить музыку", moneyDelta = -50, stressDelta = 2, resultText = "Музыка платная. Перфоратор всё равно громче." },
                    new EncounterChoice { label = "Терпеть", moneyDelta = 0, stressDelta = 5, resultText = "Ты терпишь. Терпение конвертируется в стресс." }
                }
            };
        }

        private static EncounterDefinition ParcelScam()
        {
            return new EncounterDefinition
            {
                id = "proc_parcel_scam",
                title = "Посылка",
                body = "Звонок: «Вам посылка. Нужна предоплата доставки и кода из СМС».",
                allowedLocations = new List<int> { (int)LocationId.Apartment, (int)LocationId.Office },
                weight = 3,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Сказать код", moneyDelta = -2000, stressDelta = 8, resultText = "Код ушёл. Деньги ушли. Посылка осталась в измерении ожиданий." },
                    new EncounterChoice { label = "Спросить трек-номер", moneyDelta = -200, stressDelta = 3, resultText = "Трек-номер странный, но убедительный. Ты оплатил «уточнение»." },
                    new EncounterChoice { label = "Сбросить", moneyDelta = 0, stressDelta = 2, resultText = "Ты сбросил. Потом тебе 17 раз перезвонили с разных номеров." }
                }
            };
        }

        private static EncounterDefinition KioskBait()
        {
            return new EncounterDefinition
            {
                id = "proc_kiosk_bait",
                title = "Киоск: акция",
                body = "На витрине «2 по цене 3». Продавец говорит: «Выгодно же».",
                allowedLocations = new List<int> { (int)LocationId.Kiosk },
                weight = 4,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Взять «акцию»", moneyDelta = -260, stressDelta = 1, resultText = "Ты взял «акцию». Внутри оказалось, что ты купил право купить ещё раз." },
                    new EncounterChoice { label = "Спросить, почему 2=3", moneyDelta = -50, stressDelta = 2, resultText = "Тебе объяснили маркетинг. Объяснение платное." },
                    new EncounterChoice { label = "Уйти", moneyDelta = 0, stressDelta = 1, resultText = "Ты ушёл. Тебя всё равно догнал запах шаурмы и чувство вины." }
                }
            };
        }

        private static EncounterDefinition BarTalk()
        {
            return new EncounterDefinition
            {
                id = "proc_bar_talk",
                title = "Бар: разговор",
                body = "Незнакомец предлагает «по-братски вложиться» в идею, которая звучит как беда.",
                allowedLocations = new List<int> { (int)LocationId.Bar },
                weight = 3,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Скинуться", moneyDelta = -1000, stressDelta = 2, resultText = "Ты скинулся. Идея исчезла вместе с незнакомцем и твоими деньгами." },
                    new EncounterChoice { label = "Слушать дальше", moneyDelta = -200, stressDelta = 3, resultText = "Ты слушал. За аренду ушей выставили счёт." },
                    new EncounterChoice { label = "Свалить", moneyDelta = -120, stressDelta = 1, resultText = "Ты ушёл. Бар списал с тебя «плату за атмосферу»." }
                }
            };
        }

        private static EncounterDefinition PharmacyStickerShock()
        {
            return new EncounterDefinition
            {
                id = "proc_pharmacy_shock",
                title = "Аптека: ценник",
                body = "Ты смотришь на ценник и понимаешь: здоровье — это роскошь.",
                allowedLocations = new List<int> { (int)LocationId.Pharmacy, (int)LocationId.Hospital },
                weight = 3,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Купить самое дешёвое", moneyDelta = -320, stressDelta = -1, resultText = "Ты купил самое дешёвое. На упаковке написано: «не помогает, но красиво»." },
                    new EncounterChoice { label = "Попросить аналог", moneyDelta = -180, stressDelta = 1, resultText = "Аналог есть. Но он «почти такой же», особенно по цене." },
                    new EncounterChoice { label = "Уйти", moneyDelta = 0, stressDelta = 2, resultText = "Ты ушёл. Организм записал это как предательство." }
                }
            };
        }

        private static EncounterDefinition PawnshopDeal()
        {
            return new EncounterDefinition
            {
                id = "proc_pawnshop_deal",
                title = "Ломбард",
                body = "Вывеска мигает. Внутри оценивают твои вещи и твою самооценку.",
                allowedLocations = new List<int> { (int)LocationId.Pawnshop },
                weight = 2,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Сдать что-то ценное", moneyDelta = +800, stressDelta = 4, resultText = "Ты получил деньги. Чуть позже понял, что сдал нужное." },
                    new EncounterChoice { label = "Посмотреть «выгодные предложения»", moneyDelta = -400, stressDelta = 2, resultText = "Ты купил «почти новый» чайник. Он был новым где-то в 1993." },
                    new EncounterChoice { label = "Уйти", moneyDelta = 0, stressDelta = 1, resultText = "Ты ушёл. Ломбард посмотрел тебе в спину как банк." }
                }
            };
        }

        private static EncounterDefinition DkPoster()
        {
            return new EncounterDefinition
            {
                id = "proc_dk_poster",
                title = "ДК: афиша",
                body = "Возле ДК афиши: «Дискотека 90х», «Ярмарка мёда», «Сеанс гипноза».",
                allowedLocations = new List<int> { (int)LocationId.PalaceOfCulture },
                weight = 3,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Пойти на «гипноз»", moneyDelta = -500, stressDelta = 2, resultText = "Тебя загипнотизировали купить фото с собой. Фото тоже платное." },
                    new EncounterChoice { label = "Пойти на «дискотеку 90х»", moneyDelta = -300, stressDelta = -1, resultText = "Ты вспомнил, что у тебя не было 90х. Но музыка помогла на 3 минуты." },
                    new EncounterChoice { label = "Уйти", moneyDelta = 0, stressDelta = 1, resultText = "Ты ушёл. Афиша смотрела вслед как судьба." }
                }
            };
        }

        private static EncounterDefinition RailCrossingDelay()
        {
            return new EncounterDefinition
            {
                id = "proc_rail_crossing",
                title = "Переезд",
                body = "Шлагбаум опустился. Поезда нет. Время идёт. Ты стареешь.",
                allowedLocations = new List<int> { (int)LocationId.RailCrossing, (int)LocationId.IndustrialZone },
                weight = 3,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Ждать", moneyDelta = 0, stressDelta = 3, resultText = "Ты ждал. Поезда не было. Но ожидание было реальным." },
                    new EncounterChoice { label = "Обойти", moneyDelta = -400, stressDelta = 6, resultText = "Ты обошёл. С тебя «штраф за творческий подход»." },
                    new EncounterChoice { label = "Купить кофе у дедушки", moneyDelta = -120, stressDelta = -1, resultText = "Кофе крепкий. Переезд всё равно крепче." }
                }
            };
        }

        private static EncounterDefinition BusStationSnack()
        {
            return new EncounterDefinition
            {
                id = "proc_bus_station_snack",
                title = "Автовокзал",
                body = "На автовокзале продают пирожки, которые видели много дорог и мало санитарии.",
                allowedLocations = new List<int> { (int)LocationId.BusStation },
                weight = 3,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Купить пирожок", moneyDelta = -70, stressDelta = -1, resultText = "Пирожок вкусный. Организм подписал акт протеста на завтра." },
                    new EncounterChoice { label = "Купить воду", moneyDelta = -50, stressDelta = 0, resultText = "Вода есть. Цена — как у топлива. Почти." },
                    new EncounterChoice { label = "Ничего не брать", moneyDelta = 0, stressDelta = 1, resultText = "Ты не взял. Желудок обиделся и начал переговоры." }
                }
            };
        }

        private static EncounterDefinition MineGateShift()
        {
            return new EncounterDefinition
            {
                id = "proc_mine_gate",
                title = "Проходная",
                body = "У проходной шахты охранник смотрит на тебя как на недостающую справку.",
                allowedLocations = new List<int> { (int)LocationId.MineGate, (int)LocationId.IndustrialZone },
                weight = 2,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { label = "Спросить дорогу", moneyDelta = -50, stressDelta = 2, resultText = "Дорогу подсказали. За консультацию выписали квитанцию." },
                    new EncounterChoice { label = "Попросить подработку", moneyDelta = -120, stressDelta = 3, resultText = "Подработка есть: «принеси справку, что ты человек». Справка платная." },
                    new EncounterChoice { label = "Уйти", moneyDelta = 0, stressDelta = 1, resultText = "Ты ушёл. Вдали гудок. Кажется, город одобрил твой уход." }
                }
            };
        }
    }
}
