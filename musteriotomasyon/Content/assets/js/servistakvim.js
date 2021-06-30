
var FullCalendarBasic = function () {

    var _componentFullCalendarBasic = function () {
        if (!$().fullCalendar) {
            console.warn('Warning - fullcalendar.min.js is not loaded.');
            return;
        }
        window.events = [{  }];
        var obj = {};
        var url = '/Servisler/ServisleriListele';

        $.getJSON(url, function (data) {
            for (var item in data.Result) {
                events.push({
                    "title": data.Result[item].title,
                    "start": data.Result[item].start,
                    "color": data.Result[item].durum == '0' ? '#d32020' : '#35b7de',
                    "url": data.Result[item].url
                });
            }
            abc(events);
        });
        function abc(events) {
            $('.fullcalendar-basic').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,basicWeek,basicDay'
                },

                defaultDate: Date.now(),
                editable: false,
                events: events,
                eventLimit: true,
                isRTL: $('html').attr('dir') == 'rtl' ? true : false
            });

            // Agenda view
            $('.fullcalendar-agenda').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },

                defaultDate: '2014-11-12',
                defaultView: 'agendaWeek',
                editable: false,
                businessHours: true,
                events: events,

                isRTL: $('html').attr('dir') == 'rtl' ? true : false
            });

            // List view
            $('.fullcalendar-list').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'listDay,listWeek,listMonth'
                },
                views: {
                    listDay: { buttonText: 'Gün' },
                    listWeek: { buttonText: 'Hafta' },
                    listMonth: { buttonText: 'Ay' }
                },
                defaultView: 'listMonth',
                defaultDate: '2020-02-02',
                navLinks: true, // can click day/week names to navigate views
                editable: false,
                eventLimit: true, // allow "more" link when too many events
                events: events,
                isRTL: $('html').attr('dir') == 'rtl' ? true : false
            });

            // Event colors
            $('.fullcalendar-event-colors').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                defaultDate: '2014-11-12',
                editable: false,
                events: eventColors,
                isRTL: $('html').attr('dir') == 'rtl' ? true : false
            });

            // Event background colors
            $('.fullcalendar-background-colors').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                defaultDate: '2014-11-12',
                editable: false,
                events: eventBackgroundColors,
                isRTL: $('html').attr('dir') == 'rtl' ? true : false
            });
        };

    }
    return {
        init: function () {
            _componentFullCalendarBasic();
        }
    }
}();

document.addEventListener('DOMContentLoaded', function () {
    FullCalendarBasic.init();
});

