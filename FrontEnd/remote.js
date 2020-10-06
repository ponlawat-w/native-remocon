$(document).ready(() => {
  const $btns = $('.btn-control');
  const $volume = $('#btn-volume');
  
  const query = new URLSearchParams(window.location.search);
  let accessToken;
  const urlBase = window.location.origin;
  if (query.has('token')) {
    accessToken = query.get('token');
    localStorage.setItem('accessToken', accessToken);
    window.history.replaceState({}, document.title, "/www/remote.html");
  } else if (localStorage.getItem('accessToken')) {
    accessToken = localStorage.getItem('accessToken');
  } else {
    throw 'Error getting token';
  }
  axios.defaults.headers.Authorization = `Bearer ${accessToken}`;

  const setVolume = volume => {
    volume = volume < 0 ? 0 : volume;
    volume = volume > 99 ? 99 : volume;
    $volume.html(volume.toString().padStart(2, '0'));
  };

  const init = async() => {
    try {
      const response = await axios.get(`${urlBase}/api/audio/get`);
      setVolume(response.data);
    } finally {}
  };

  $btns.click(async e => {
    const $target = $(e.target).closest('button');
    if ($target.attr('data-control')) {
      try {
        $btns.prop('disabled', true);
        const response = await axios.get(`${urlBase}/api/${$target.attr('data-control')}`);
        if ($target.attr('data-update-volume')) {
          setVolume(response.data);
        }
      } finally {
        $btns.prop('disabled', false);
      }
    }
  });

  init();
  document.body.style.visibility = 'initial';
});

