new Vue({
  el: '#vue',
  data: {
    accessToken: null,
    urlBase: null,
    loading: false
  },
  methods: {
    control: async function(e) {
      e.preventDefault();
      e.stopPropagation();
      e.stopImmediatePropagation();
      e.target.blur();
      if (e.target.hasAttribute('control')) {
        this.loading = true;
        try {
          await axios.get(`${this.urlBase}/api/${e.target.getAttribute('control')}`);
          const el = document.querySelector(':focus');
          if (el) {
            el.blur();
          }
        } finally {
          this.loading = false;
        }
      }
    }
  },
  mounted: function() {
    const query = new URLSearchParams(window.location.search);
    if (query.has('token')) {
      this.accessToken = query.get('token');
      this.urlBase = window.location.origin;
      // window.history.replaceState({}, document.title, "/www/access.html");
      axios.defaults.headers.Authorization = `Bearer ${this.accessToken}`;
    }
  }
});

document.body.style.visibility = 'initial';
