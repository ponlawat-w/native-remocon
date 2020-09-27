new Vue({
  el: '#vue',
  data: {
    accessToken: '',
    networks: [],
    loading: true,
    ip: null,
    updateQr: false,
    qr: null
  },
  computed: {
    accessUrl: function() {
      return this.ip ? `https://${this.ip}:${window.location.port}/www/remote.html?token=${this.accessToken}` : null;
    }
  },
  methods: {
    saveNetwork: function() {
      localStorage.setItem('network', this.ip);
    }
  },
  watch: {
    ip: function() {
      if (this.ip) {
        this.updateQr = true;
      }
    }
  },
  updated: function() {
    if (this.updateQr) {
      if (this.qr) {
        this.qr.clear();
        this.qr.makeCode(this.accessUrl);
      } else {
        this.qr = new QRCode('qr', {
          text: this.accessUrl
        });
      }
      this.updateQr = false;
    }
  },
  mounted: async function() {
    try {
      const query = new URLSearchParams(window.location.search);
      if (query.has('token')) {
        this.accessToken = query.get('token');
        window.history.replaceState({}, document.title, "/www/access.html");
        const results = await axios.get(`${window.location.origin}/api/network/list`, {
          headers: {
            Authorization: `Bearer ${this.accessToken}`
          }
        });
        this.networks = results.data;
        if (this.networks.length) {
          const savedNetwork = localStorage.getItem('network');
          if (savedNetwork) {
            for (let i = 0; i < this.networks.length; i++) {
              if (this.networks[i].address === savedNetwork) {
                this.ip = this.networks[i].address;
                break;
              }
            }
          }
          if (!this.ip) {
            this.ip = this.networks[0].address;
          }
        }
      }
    } finally {
      this.loading = false;
    }
  }
});

document.body.style.visibility = 'initial';
